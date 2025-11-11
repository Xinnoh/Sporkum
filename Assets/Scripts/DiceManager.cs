using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{


    private HorizontalCardHolder playerHolder, enemyHolder, playerMoves, enemyMoves;



    private void Start()
    {
        playerHolder = CardHolderRegistry.Instance.playerCharacterHolder;
        enemyHolder = CardHolderRegistry.Instance.enemyCharacterHolder;
        playerMoves = CardHolderRegistry.Instance.playerMoveHolder;
        enemyMoves = CardHolderRegistry.Instance.enemyMoveHolder;
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
        RerollDiceInHolder(playerHolder);
    }

    public void RerollEnemyDice()
    {
        RerollDiceInHolder(enemyHolder);
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
