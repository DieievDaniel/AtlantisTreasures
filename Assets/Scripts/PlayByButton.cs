using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayByButton : MonoBehaviour
{
    [SerializeField] private SpawnGrid spawnGrid;
    [SerializeField] private TextOutput textOutput;
    [SerializeField] private MoneyOperation moneyOperation;
    [SerializeField] private AutoPlay autoPlay;

    public TextMeshProUGUI RollTextButton;
    [SerializeField] private Button increaseRollButton;
    [SerializeField] private Button decreaseRollButton;

    public Button IncreaseRollButton
    {
        get { return increaseRollButton; }
        set { increaseRollButton = value; }
    }

    public Button DecreaseRollButton
    {
        get { return decreaseRollButton; }
        set { decreaseRollButton = value; }
    }


    public void RollClick()
    {
        spawnGrid.SpawnMove();
        moneyOperation.BetOperation();
    }

    public void IncreaseRolling()
    {
        autoPlay.NumberOfAutoPlaying += 1;
        textOutput.UIOutput();
    }

    public void DeacreaseRolling()
    {
        autoPlay.NumberOfAutoPlaying -= 1;
        textOutput.UIOutput();
    }
}
