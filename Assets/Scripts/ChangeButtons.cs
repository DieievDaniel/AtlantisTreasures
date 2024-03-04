using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtons : MonoBehaviour
{
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private MoneyOperation moneyOperation;
    [SerializeField] private SpawnGrid spawnGrid;
    [SerializeField] private AutoPlay autoPlay;
    [SerializeField] private PlayByButton playByButton;
    [SerializeField] private Button backToMenu;
    [SerializeField] private Button spinWheel;

    public Button SpinWheel
    {
        get { return spinWheel; }
        set { spinWheel = value; }
    }

    private void Update()
    {
        CheckButtonStatus();
    }

    public void DeactivateButtons()
    {
        SetButtonInteractability(false);
    }

    public void ActivateButtons()
    {
        SetButtonInteractability(true);
    }

    private void SetButtonInteractability(bool interactable)
    {
        foreach (var button in buttons)
        {
            button.interactable = interactable;
        }
    }

    public void CheckButtonStatus()
    {
        // Проверяем баланс и состояние автоспина
        if (moneyOperation.Balance < moneyOperation.Bet || AutoPlay.autoSpawning)
        {
            DeactivateButtons();
            return; // Не продолжаем проверку, если условие выполнено
        }

        // Проверяем количество спинов
        if (spawnGrid.spawnCount < 20)
        {
            spinWheel.interactable = false;
        }
        else
        {
            spinWheel.interactable = true;
        }

        // Проверяем ставку
        if (moneyOperation.Bet < 50)
        {
            foreach (var button in buttons)
            {
                if (button != moneyOperation.IncreaseBetButton)
                {
                    button.interactable = false;
                }
            }
        }

        // Проверяем автоспин
        if (autoPlay.NumberOfAutoPlaying <= 0)
        {
            foreach (var button in buttons)
            {
                if (button != playByButton.IncreaseRollButton)
                {
                    button.interactable = false;
                }
            }
        }

        // Если ни одно из вышеуказанных условий не выполнено, активируем все кнопки
        if (moneyOperation.Bet >= 50 && autoPlay.NumberOfAutoPlaying > 0 && spawnGrid.spawnCount >= 20)
        {
            ActivateButtons();
        }
    }

}
