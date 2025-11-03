using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public HorizontalCardHolder moveHolder;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RerollDice();
        }
    }



    public void RerollDice()
    {
        foreach (var dice in GameObject.FindGameObjectsWithTag("Dice"))
            dice.GetComponent<Dice>().RerollValue();

        moveHolder.SyncDiceValues();
    }



}
