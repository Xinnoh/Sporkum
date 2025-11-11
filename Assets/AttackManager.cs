using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private HorizontalCardHolder playerHolder, enemyHolder;


    public CombatManager combatManager;

    private bool isPlayerTurn;

    void Start()
    {
        playerHolder = CardHolderRegistry.Instance.playerCharacterHolder;
        enemyHolder = CardHolderRegistry.Instance.enemyCharacterHolder;
    }


    private IEnumerator AttackPhase(HorizontalCardHolder holder, string nextPhaseDebug)
    {
        // Sort the cards into turn order, then execute them
        List<Card> attackOrder = new List<Card>();

        foreach (var card in holder.cards)
        {
            if (card != null && card.attackHandler != null)
                attackOrder.Add(card);
        }

        attackOrder.Sort((a, b) =>
        isPlayerTurn
            ? b.ParentIndex().CompareTo(a.ParentIndex())  // player: right to left
            : a.ParentIndex().CompareTo(b.ParentIndex())  // enemy: left to right
    );


        foreach (var card in attackOrder)
        {
            yield return StartCoroutine(card.attackHandler.PerformAttack());

            if (IsBattleOver())
                yield break;

            yield return new WaitForSeconds(0.3f);
        }

    }

    public IEnumerator PlayerAttackPhase()
    {
        isPlayerTurn = true;
        yield return AttackPhase(playerHolder, "Player attacks done Å® Enemy turn");
        combatManager.SetCombatState(CombatState.EnemyAnim);
    }

    public IEnumerator EnemyAttackPhase()
    {
        isPlayerTurn = false;
        yield return AttackPhase(enemyHolder, "Enemy attacks done Å® Back to player");
        combatManager.SetCombatState(CombatState.PlayerTurn);
    }
    bool IsBattleOver()
    {
        if (AllDead(playerHolder))
        {
            combatManager.SetStateLose();
            return true;
        }

        if (AllDead(enemyHolder))
        {
            combatManager.SetStateWin();
            return true;
        }

        return false;
    }

    bool AllDead(HorizontalCardHolder holder)
    {
        if (holder == null || holder.cards == null) return true;

        foreach (var card in holder.cards)
        {
            if (card == null) continue;

            var health = card.GetComponent<HealthManager>();
            if (health != null && health.IsAlive())
                return false;
        }

        return true;
    }
}
