using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{


    private HorizontalCardHolder playerHolder, enemyHolder, playerMoves, enemyMoves, playerDice, enemyDice;



    private void Start()
    {
        playerHolder = CardHolderRegistry.Instance.playerCharacterHolder;
        enemyHolder = CardHolderRegistry.Instance.enemyCharacterHolder;
        playerMoves = CardHolderRegistry.Instance.playerMoveHolder;
        enemyMoves = CardHolderRegistry.Instance.enemyMoveHolder;
        playerDice = CardHolderRegistry.Instance.playerDiceHolder;
        enemyDice = CardHolderRegistry.Instance.enemyDiceHolder;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RerollPlayerDice();
            RerollEnemyDice();
        }
    }



    public void RerollPlayerDice()
    {
        RerollDiceInHolder(playerDice);
    }

    public void RerollEnemyDice()
    {
        RerollDiceInHolder(enemyDice);
    }

    private void RerollDiceInHolder(HorizontalCardHolder holder)
    {
        if (holder == null || holder.cards == null) return;

        foreach (var card in holder.cards)
        {
            if (card == null) continue;

            var dice = card.GetComponent<Dice>();

            if (dice != null)
            {
                dice.RerollValue();

            }
        }
        playerMoves.SyncMovesToDice();
        enemyMoves.SyncMovesToDice();
    }


}
