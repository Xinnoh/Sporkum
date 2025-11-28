using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private DiceManager diceManager;

    private HorizontalCardHolder[] playerCardHolders, enemyCardHolders;
    public AttackManager attackManager;


    public CombatState combatState;
    private bool canSelectCards = true;

    public bool combatOver;

    private CombatState? requestedState = null;


    void Start()
    {
        diceManager = GetComponent<DiceManager>();
        playerCardHolders = CardHolderRegistry.Instance.playerHolders;
        enemyCardHolders = CardHolderRegistry.Instance.enemyHolders;

        enemyCardHolders[0].SetTurnActive(false);
        enemyCardHolders[1].SetTurnActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCombat();
        }
    }


    public void StartCombat()
    {
        foreach (var holder in playerCardHolders)
            holder.StartCombat();

        foreach (var holder in enemyCardHolders)
            holder.StartCombat();

        combatOver = false;
        combatState = CombatState.Intro;
        StartCoroutine(CombatFlow());
    }


    IEnumerator CombatFlow()
    {
        yield return StartCoroutine(CombatIntro());

        while (combatState != CombatState.PlayerWin && combatState != CombatState.PlayerLose)
        {
            yield return StartCoroutine(PlayerTurn());
            if (requestedState.HasValue)
            {
                combatState = requestedState.Value;
                requestedState = null;
                continue;
            }

            yield return StartCoroutine(PlayerAnim());

            if (!CheckPlayerWin())
            {
                yield return StartCoroutine(EnemyAnim());


            }
        }

        yield return StartCoroutine(CombatEnd());
    }


    IEnumerator CombatIntro()
    {
        combatState = CombatState.Intro;
        SetTurnActive(false);
        Debug.Log("Combat Intro started.");

        yield return new WaitForSeconds(0.5f);


        // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A) || requestedState.HasValue);

        TryApplyRequestedState();

        combatState = CombatState.PlayerTurn;
    }

    IEnumerator PlayerTurn()
    {
        combatState = CombatState.PlayerTurn;
        SetTurnActive(true);

        Debug.Log("Player Turn started.");

        yield return new WaitUntil(() => requestedState.HasValue);

        TryApplyRequestedState();

        canSelectCards = false;
    }

    IEnumerator PlayerAnim()
    {
        combatState = CombatState.PlayerAnim;
        SetTurnActive(false);

        Debug.Log("Player Animations playing.");

        yield return StartCoroutine(attackManager.PlayerAttackPhase());


        yield return new WaitUntil(() => requestedState.HasValue);


        TryApplyRequestedState();

        Debug.Log("Player Animations finished.");
    }

    IEnumerator EnemyAnim()
    {
        combatState = CombatState.EnemyAnim;

        Debug.Log("Enemy Animations playing.");

        yield return StartCoroutine(attackManager.EnemyAttackPhase());

        yield return new WaitUntil(() => requestedState.HasValue);

        Debug.Log("Enemy Animations finished.");


        // Loop back if not win
        if (!CheckPlayerWin() && !CheckEnemyWin())
        {
            diceManager.RerollEnemyDice();
            diceManager.RerollPlayerDice();
            SetCombatState(CombatState.PlayerTurn);
        }

        TryApplyRequestedState();
    }

    public void RequestCombatState(CombatState newState)
    {
        requestedState = newState;
    }

    bool TryApplyRequestedState()
    {
        if (requestedState.HasValue)
        {
            combatState = requestedState.Value;
            requestedState = null;
            return true;
        }
        return false;
    }

    IEnumerator CombatEnd()
    {
        if (combatState == CombatState.PlayerWin)
        {
            Debug.Log("Player Wins!G");
        }
        else
        {
            Debug.Log("Player Loses!G");
        }

        yield return new WaitUntil(() => requestedState.HasValue);

        Debug.Log("Combat Ended.");
    }

    bool CheckPlayerWin()
    {
        return CheckCombatOver(true);
    }

    bool CheckEnemyWin()
    {
        return CheckCombatOver(false);
    }

    bool CheckCombatOver(bool isEnemy)
    {
        HorizontalCardHolder holderToCheck = isEnemy
            ? enemyCardHolders[0]
            : playerCardHolders[0];

        if (holderToCheck.cards.Count == 0)
            return true;

        // If any card is alive, return false
        foreach (var card in holderToCheck.cards)
        {
            if (card != null && card.GetComponent<HealthHandler>().IsAlive())
                return false;
        }

        return true;
    }

    public void CheckWinCondition()
    {

    }

    public void SetTurnActive(bool active)
    {
        foreach (var holder in playerCardHolders)
            holder.SetTurnActive(active);
    }

    public bool IsTurnActive()
    {
        return canSelectCards;
    }
    public void SetStateIntro() => SetCombatState(CombatState.Intro);
    public void SetStatePlayerTurn() => SetCombatState(CombatState.PlayerTurn);
    public void SetStateEndTurn() => SetCombatState(CombatState.PlayerAnim);
    public void SetStateEnemyAnim() => SetCombatState(CombatState.EnemyAnim);
    public void SetStateWin() => SetCombatState(CombatState.PlayerWin);
    public void SetStateLose() => SetCombatState(CombatState.PlayerLose);

    public void SetCombatState(CombatState newState)
    {
        requestedState = newState;
        Debug.Log("Combat state set to: " + newState);
    }
}
