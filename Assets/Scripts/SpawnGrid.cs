using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PrefabData
{
    public GameObject prefab;
    public int id;
}

[System.Serializable]
public class BackgroundData
{
    public GameObject prefab;
    public Vector2 size;
}

public class SpawnGrid : MonoBehaviour
{
    [SerializeField] private PrefabData[] objectPrefabs;
    [SerializeField] private BackgroundData[] backgroundPrefabs;
    [SerializeField] private Vector2[] backgroundSizes;
    [SerializeField] private float spacing;
    [SerializeField] private float elementSize;
    [SerializeField] private float spawnDuration;
    [SerializeField] private MoneyOperation moneyOperation;
    [SerializeField] private ButtonManager buttonManager;

    public int spawnCount = 0;
    private RectTransform rectTransform;
    public bool isSpawning = false;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private DestroyAndRespawnElements destroyAndRespawnElements;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        destroyAndRespawnElements = GetComponent<DestroyAndRespawnElements>();
        SpawnBackgrounds();
    }

    public void SpawnMove()
    {
        if (!isSpawning)
        {
            ClearElements();
            moneyOperation.Winnings = 0;
            StartCoroutine(SpawnAndMove());
        }
    }

    public void ClearElements()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj.tag != "Background")
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
    }

    private IEnumerator Move(GameObject obj, Vector3 targetPosition, float moveDuration)
    {
        Vector3 startPosition = obj.transform.localPosition;
        float moveTime = 0f;

        while (moveTime < moveDuration)
        {
            moveTime += Time.deltaTime;
            float t = moveTime / moveDuration;
            obj.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        obj.transform.localPosition = targetPosition;
    }

    private IEnumerator SpawnAndMove()
    {
        isSpawning = true;
        spawnCount++;

        int row = GridsSize.Rows;
        int column = GridsSize.Columns;

        float totalWidth = column * spacing;
        float totalHeight = row * spacing;

        float startX = -totalWidth / 2f + spacing / 2f;
        float startY = totalHeight / 2f - spacing / 2f;

        bool spawnDownwards = true;

        for (int i = 0; i < column; i++)
        {
            int start = spawnDownwards ? 0 : row - 1;
            int end = spawnDownwards ? row : -1;
            int step = spawnDownwards ? 1 : -1;

            for (int j = start; j != end; j += step)
            {
                int randomIndex = Random.Range(0, objectPrefabs.Length);
                GameObject newObject = Instantiate(objectPrefabs[randomIndex].prefab, Vector3.zero, Quaternion.identity);
                newObject.transform.SetParent(rectTransform, false);
                newObject.transform.localScale = new Vector3(elementSize, elementSize, 1f);
                newObject.transform.localPosition = new Vector3(startX + i * spacing, startY - j * spacing, 0f);
                spawnedObjects.Add(newObject);

                StartCoroutine(Move(newObject, new Vector3(startX + i * spacing, startY - j * spacing, 0f), spawnDuration));
                yield return new WaitForSeconds(0.05f);
            }

            spawnDownwards = !spawnDownwards;
        }

        yield return new WaitForSeconds(spawnDuration);

        destroyAndRespawnElements.RemoveAndRespawnElements(objectPrefabs, rectTransform, row, column, spacing, elementSize, spawnDuration, spawnedObjects);
    }

    private void SpawnBackgrounds()
    {
        int row = GridsSize.Rows;
        int column = GridsSize.Columns;

        int backgroundIndex = ChooseBackgroundIndex(row, column);
        if (backgroundIndex != -1 && backgroundIndex < backgroundPrefabs.Length)
        {
            GameObject background = Instantiate(backgroundPrefabs[backgroundIndex].prefab, Vector3.zero, Quaternion.identity);
            background.transform.SetParent(rectTransform, false);
            background.transform.localScale = new Vector3(backgroundSizes[backgroundIndex].x, backgroundSizes[backgroundIndex].y, 1f);
        }
    }

    private int ChooseBackgroundIndex(int row, int column)
    {
        if (row == 3 && column == 4)
        {
            return 0;
        }
        else if (row == 4 && column == 5)
        {
            return 1;
        }
        else if (row == 5 && column == 6)
        {
            return 2;
        }
        else if (row == 6 && column == 7)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }
}
