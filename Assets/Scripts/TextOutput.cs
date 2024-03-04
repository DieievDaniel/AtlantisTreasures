using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOutput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI betText;
    [SerializeField] private TextMeshProUGUI winningsText;

    [SerializeField] private MoneyOperation moneyOperation;
    [SerializeField] private PlayByButton playByButton;
    [SerializeField] private AutoPlay autoPlay;


    private void Start()
    {
        UIOutput();
    }
    public void UIOutput()
    {
        balanceText.text = "" + moneyOperation.Balance.ToString();
        betText.text = "" + moneyOperation.Bet.ToString();
        winningsText.text = "" + moneyOperation.Winnings.ToString();
        playByButton.RollTextButton.text = "" + autoPlay.NumberOfAutoPlaying.ToString();
    }
}
