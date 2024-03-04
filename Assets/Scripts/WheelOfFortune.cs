using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] private TextOutput textOutput;
    [SerializeField] private MoneyOperation moneyOperation;
    [SerializeField] private SpawnGrid spawnGrid;
    [SerializeField] private Button wheelButton;
    [SerializeField] private GameObject fortuneMenu;
    [SerializeField] private GameObject wheelOfFortune;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private List<GameObject> objectsToExclude; // Список объектов для исключения
    [SerializeField] private GameObject[] winObjects; // Массив объектов для выигрышей

    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private float spinDuration = 3f;

    private bool isSpinning = false;

    void Start()
    {
        foreach (GameObject winObject in winObjects)
        {
            winObject.SetActive(false);
        }
    }
    public void OpenWheelMenu()
    {
        gameScreen.SetActive(false);
        fortuneMenu.SetActive(true);
        SetObjectsActive(true); // Включаем объекты, которые не должны быть отключены
    }

    public void CloseWheelMenu()
    {
        fortuneMenu.SetActive(false);
        gameScreen.SetActive(true);
        SetObjectsActive(false); // Отключаем объекты, которые не должны быть отключены
    }

    public void SpinWheel()
    {
        if (!isSpinning && spawnGrid.spawnCount >= 10)
        {
            spawnGrid.spawnCount = 0;
            StartCoroutine(SpinWheelCoroutine());
        }
    }

    private void SetObjectsActive(bool active)
    {
        foreach (var obj in objectsToExclude)
        {
            obj.SetActive(active);
        }
    }

    private IEnumerator SpinWheelCoroutine()
    {
        isSpinning = true;

        float elapsedTime = 0f;
        float startAngle = wheelOfFortune.transform.eulerAngles.z;
        float stopAngle = 360f / 6f * Random.Range(0, 6) + 1080f; // Вычисляем угол для остановки вращения

        while (elapsedTime < spinDuration)
        {
            float angle = Mathf.Lerp(startAngle, stopAngle, rotationCurve.Evaluate(elapsedTime / spinDuration));
            wheelOfFortune.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wheelOfFortune.transform.rotation = Quaternion.Euler(0f, 0f, stopAngle);

        DetermineResult(stopAngle);

        isSpinning = false;
    }

    private void DetermineResult(float stopAngle)
    {
        int segmentNumber = Mathf.FloorToInt((stopAngle - 1080f) / (360f / 6f));
        switch (segmentNumber)
        {
            case 0:
                Debug.Log("Win: 1000");
                moneyOperation.Balance += 1000;
                textOutput.UIOutput();
                ActivateWinObject(5); // Активируем соответствующий объект выигрыша
                break;
            case 1:
                Debug.Log("Win: x4");
                moneyOperation.IsX4Active = true;
                ActivateWinObject(3); // Активируем соответствующий объект выигрыша
                break;
            case 2:
                Debug.Log("Win: 5000");
                moneyOperation.Balance += 5000;
                textOutput.UIOutput();
                ActivateWinObject(4); // Активируем соответствующий объект выигрыша
                break;
            case 3:
                Debug.Log("Lose");
                ActivateWinObject(0); // Активируем соответствующий объект выигрыша
                break;
            case 4:
                Debug.Log("Win: Respin");
                spawnGrid.spawnCount = 20;
                ActivateWinObject(1);
                Debug.Log(1);
                break;
            case 5:
                Debug.Log("x2");
                moneyOperation.IsX2Active = true;
                ActivateWinObject(2); // Активируем соответствующий объект выигрыша
                break;
            default:
                Debug.Log("Invalid segment number");
                break;
        }
    }

    // Метод для активации объекта выигрыша из массива
    private void ActivateWinObject(int index)
    {
        if (index >= 0 && index < winObjects.Length)
        {
            StartCoroutine(ShowWinObjectForDuration(winObjects[index], 2f));
        }
    }

    private IEnumerator ShowWinObjectForDuration(GameObject obj, float duration)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
    }
}
