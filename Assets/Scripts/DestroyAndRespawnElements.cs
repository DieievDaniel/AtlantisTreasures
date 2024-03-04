using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class DestroyAndRespawnElements : MonoBehaviour
{
    [SerializeField] private SpawnGrid spawnGrid;
    [SerializeField] private MoneyOperation moneyOperation;
    [SerializeField] private TextOutput textOutput;

    public void RemoveAndRespawnElements(PrefabData[] prefabs, RectTransform rectTransform, int row, int column, float spacing, float elementSize, float spawnDuration, List<GameObject> spawnedObjects)
    {
        Dictionary<int, List<GameObject>> objectsToRemove = new Dictionary<int, List<GameObject>>();

        foreach (var obj in spawnedObjects)
        {
            PrefabIdentifier prefabIdentifier = obj.GetComponent<PrefabIdentifier>();
            if (prefabIdentifier != null)
            {
                int id = prefabIdentifier.id;
                if (objectsToRemove.ContainsKey(id))
                {
                    objectsToRemove[id].Add(obj);
                }
                else
                {
                    objectsToRemove[id] = new List<GameObject> { obj };
                }
            }
        }

        int gridSize = ChooseGridSize(row, column); // Определение размера сетки
        int winningCount = gridSize; // Количество элементов для выигрыша

        StartCoroutine(RemoveAndRespawnCoroutine(objectsToRemove, prefabs, rectTransform, row, column, spacing, elementSize, spawnDuration, spawnedObjects, winningCount));
    }

    private IEnumerator RemoveAndRespawnCoroutine(Dictionary<int, List<GameObject>> objectsToRemove, PrefabData[] prefabs, RectTransform rectTransform, int row, int column, float spacing, float elementSize, float spawnDuration, List<GameObject> spawnedObjects, int winningCount)
    {
        bool foundWinningCombination = false;

        while (true)
        {
            foundWinningCombination = false;

            foreach (var kvp in objectsToRemove)
            {
                if (kvp.Value.Count >= winningCount)
                {
                    foundWinningCombination = true;
                    break;
                }
            }

            if (!foundWinningCombination)
                break;

            yield return new WaitForSeconds(1.0f);

            foreach (var kvp in objectsToRemove)
            {
                if (kvp.Value.Count >= winningCount)
                {
                    int elementId = kvp.Value[0].GetComponent<PrefabIdentifier>().id;
                    int count = kvp.Value.Count;
                    moneyOperation.CalculateWinnings(elementId, count, row, column);

                    foreach (var obj in kvp.Value)
                    {
                        spawnedObjects.Remove(obj);
                        StartCoroutine(ShakeObject(obj)); // Потрясываем объект перед уничтожением
                        StartCoroutine(DestroyAnimation(obj)); // Вызываем анимацию уничтожения
                    }
                    break;
                }
            }

            yield return new WaitForSeconds(1.0f);

            List<Vector3> emptyPositions = FindEmptyPositions(row, column, spacing, elementSize, spawnedObjects);
            foreach (Vector3 position in emptyPositions)
            {
                int randomIndex = Random.Range(0, prefabs.Length);
                GameObject newSprite = Instantiate(prefabs[randomIndex].prefab, position, Quaternion.identity);
                newSprite.transform.SetParent(rectTransform, false);
                newSprite.transform.localScale = new Vector3(elementSize, elementSize, 1f);
                spawnedObjects.Add(newSprite);
                StartCoroutine(RespawnAnimation(newSprite)); // Вызываем анимацию респавна для нового объекта
            }

            yield return new WaitForSeconds(1.0f);

            foreach (var kvp in objectsToRemove)
            {
                if (kvp.Value.Count >= winningCount)
                {
                    foreach (var obj in kvp.Value)
                    {
                        spawnedObjects.Remove(obj);
                        Destroy(obj);
                    }
                    break;
                }
            }

            objectsToRemove = new Dictionary<int, List<GameObject>>();
            foreach (var obj in spawnedObjects)
            {
                PrefabIdentifier prefabIdentifier = obj.GetComponent<PrefabIdentifier>();
                if (prefabIdentifier != null)
                {
                    int id = prefabIdentifier.id;
                    if (objectsToRemove.ContainsKey(id))
                    {
                        objectsToRemove[id].Add(obj);
                    }
                    else
                    {
                        objectsToRemove[id] = new List<GameObject> { obj };
                    }
                }
            }
        }
        spawnGrid.isSpawning = false;
    }

    private IEnumerator DestroyAnimation(GameObject obj)
    {
        // Проиграйте здесь анимацию уничтожения объекта
        float duration = 1f;
        float elapsedTime = 0f;
        Vector3 initialScale = obj.transform.localScale;

        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            obj.transform.localScale = initialScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }

    private IEnumerator RespawnAnimation(GameObject obj)
    {
        // Проиграйте здесь анимацию респавна объекта
        obj.SetActive(false); // Начните с отключения объекта

        // Включите объект с плавной анимацией
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 targetScale = obj.transform.localScale;

        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            obj.transform.localScale = targetScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.SetActive(true);
    }

    private IEnumerator ShakeObject(GameObject obj)
    {
        if (obj == null)
            yield break; // Прерываем корутину, если объект уже уничтожен

        float duration = 1.5f;
        float magnitude = 0.1f;
        Vector3 originalPosition = obj.transform.position;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            if (obj == null)
                yield break; // Прерываем корутину, если объект уже уничтожен

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            obj.transform.position = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        if (obj != null) // Проверяем существование объекта перед возвратом к исходной позиции
            obj.transform.position = originalPosition;
    }


    private List<Vector3> FindEmptyPositions(int row, int column, float spacing, float elementSize, List<GameObject> spawnedObjects)
    {
        List<Vector3> emptyPositions = new List<Vector3>();

        float totalWidth = column * spacing;
        float totalHeight = row * spacing;
        float startX = -totalWidth / 2f + spacing / 2f;
        float startY = totalHeight / 2f - spacing / 2f;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector3 currentPosition = new Vector3(startX + j * spacing, startY - i * spacing, 0f);
                bool isOccupied = false;

                foreach (GameObject obj in spawnedObjects)
                {
                    if (Vector3.Distance(obj.transform.localPosition, currentPosition) < 0.01f)
                    {
                        isOccupied = true;
                        break;
                    }
                }

                if (!isOccupied)
                {
                    emptyPositions.Add(currentPosition);
                }
            }
        }

        return emptyPositions;
    }

    private int ChooseGridSize(int row, int column)
    {
        if (row == 3 && column == 4)
        {
            return 5;
        }
        else if (row == 4 && column == 5)
        {
            return 7;
        }
        else if (row == 5 && column == 6)
        {
            return 8;
        }
        else if (row == 6 && column == 7)
        {
            return 9;
        }
        else
        {
            return 0;
        }
    }
}
