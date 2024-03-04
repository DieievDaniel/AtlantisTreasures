using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MoneyOperation : MonoBehaviour
{
    [SerializeField] private float balance;
    [SerializeField] private int bet = 100;
    [SerializeField] private float winnings;

    [SerializeField] private TextOutput textOutput;
    [SerializeField] public Button increaseBetButton;

    private DateTime lastBonusTime;
    private bool hasDailyBonusClaimed = false;
    private const float DailyBonusAmount = 10000f;

    private static int currentMultiplier = 1;
    [SerializeField] private static bool isX2Active = false;
    [SerializeField] private static bool isX4Active = false;

    private void Start()
    {
        LoadMoney();
        textOutput.UIOutput();
        lastBonusTime = GetLastBonusTime();
    }

    private void Update()
    {
        CheckDailyBonus();
    }

    private DateTime GetLastBonusTime()
    {
        string lastBonusTimeString = PlayerPrefs.GetString("LastBonusTime", string.Empty);
        if (!string.IsNullOrEmpty(lastBonusTimeString))
        {
            return DateTime.Parse(lastBonusTimeString);
        }
        return DateTime.Today;
    }

    private void CheckDailyBonus()
    {
        if (DateTime.Today.Subtract(lastBonusTime).TotalHours >= 24)
        {
            AddDailyBonus();
            lastBonusTime = DateTime.Today;
            PlayerPrefs.SetString("LastBonusTime", lastBonusTime.ToString());
            PlayerPrefs.Save();
            textOutput.UIOutput();
            Debug.Log(balance);
        }
    }

    private void AddDailyBonus()
    {
        balance += DailyBonusAmount;
        SaveMoney();
    }

    public float Balance
    {
        get { return balance; }
        set { balance = value; }
    }

    public Button IncreaseBetButton
    {
        get { return increaseBetButton; }
        set { increaseBetButton = value; }
    }

    public int Bet
    {
        get { return bet; }
        set { bet = value; }
    }

    public float Winnings
    {
        get { return winnings; }
        set { winnings = value; }
    }

    public void BetOperation()
    {
        if (balance >= bet)
        {
            balance -= bet;
            textOutput.UIOutput();
        }
    }

    public void CalculateWinnings(int elementId, int count, int row, int column)
    {
        float multiplier = Coefficient.GetMultiplier(elementId, count, row, column);

        if (isX2Active)
        {
            multiplier *= 2f;
            isX2Active = false; // Сбрасываем флаг после применения x2
        }
        if (isX4Active)
        {
            multiplier *= 4f;
            isX4Active = false; // Сбрасываем флаг после применения x4
        }

        winnings = bet * multiplier;
        balance += winnings;
        Debug.Log("ID:  " + elementId + ", Counts: " + count + ", Coefficient: " + multiplier + ", Winnings: " + winnings);

        if (multiplier == 2)
        {
            isX2Active = true; // Включаем режим x2 для следующего выигрыша
        }
        else if (multiplier == 4)
        {
            isX4Active = true; // Включаем режим x4 для следующего выигрыша
        }

        textOutput.UIOutput();
    }

    private void OnApplicationQuit()
    {
        SaveMoney();
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetFloat("Balance", balance);
        PlayerPrefs.Save();
    }

    public void LoadMoney()
    {
        if (PlayerPrefs.HasKey("Balance"))
        {
            balance = PlayerPrefs.GetFloat("Balance");
        }
    }

    public void IncreaseBetByTwo()
    {
        SetMultiplier(2);
    }

    public void IncreaseBetByFour()
    {
        SetMultiplier(4);
    }

    public void IncreaseBetBySix()
    {
        SetMultiplier(6);
    }

    public void IncreaseBetByEight()
    {
        SetMultiplier(8);
    }

    public void IncreaseBetByTen()
    {
        SetMultiplier(10);
    }

    private void SetMultiplier(int multiplier)
    {
        if (currentMultiplier != multiplier)
        {
            currentMultiplier = multiplier;
            bet = 100 * currentMultiplier;
            textOutput.UIOutput();
            Debug.Log(bet);
        }
        else
        {
            currentMultiplier = 1;
            bet = 100;
            textOutput.UIOutput();
            Debug.Log(bet);
        }
    }

    public static int GetCurrentMultiplier()
    {
        return currentMultiplier;
    }

    public static void SetCurrentMultiplier(int multiplier)
    {
        currentMultiplier = multiplier;
    }

    public bool IsX2Active
    {
        get { return isX2Active; }
        set { isX2Active = value; }
    }

    public bool IsX4Active
    {
        get { return isX4Active; }
        set { isX4Active = value; }
    }
}
