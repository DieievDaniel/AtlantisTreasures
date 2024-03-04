using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    [SerializeField] private int numberOfAutoPlaying;
    [SerializeField] private int currentAutoPlaying;
    [SerializeField] private MoneyOperation moneyOperation;
    [SerializeField] private SpawnGrid spawnGrid;
    [SerializeField] private TextOutput textOutput;
    [SerializeField] private ChangeButtons changeButtons;
    private int totalBet;

    public static bool autoSpawning = false;

    public int NumberOfAutoPlaying
    {
      get { return numberOfAutoPlaying; }
      set { numberOfAutoPlaying = value; }
    }

    public int TotalBet
    {
        get { return totalBet; }
        set { totalBet = value; }
    }
    public void AutoSpawnGrid()
    {

        totalBet = moneyOperation.Bet * numberOfAutoPlaying;
        if (moneyOperation.Balance >= totalBet)
        {
            StartCoroutine(Spawning());
        }

    }

    private IEnumerator Spawning()
    {
        autoSpawning = true;
        currentAutoPlaying = 0;
        while (currentAutoPlaying < numberOfAutoPlaying)
        {
            moneyOperation.Balance -= moneyOperation.Bet;
            textOutput.UIOutput();
            spawnGrid.SpawnMove();
            yield return new WaitForSeconds(3f); 

            yield return new WaitUntil(() => !spawnGrid.isSpawning); 
            currentAutoPlaying++;
        }
        autoSpawning = false;
        //moneyOperation.Bet = 100;

    }
}
