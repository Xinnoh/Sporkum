using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private Dice dice;
    private CharacterData characterData;
    private Card card;
    private Animator animator;
    private CardVisual cardVisual;

    private HorizontalCardHolder charHolder, moveHolder, diceHolder, otherCharHolder;

    private int currentIndex;

    private bool initialised;


    public void Initialise()
    {
        animator = GetComponentInChildren<Animator>();
        card = GetComponent<Card>();
        characterData = card.characterData;
        cardVisual = card.cardVisual;

        currentIndex = card.ParentIndex();

        if (!card.isEnemy)
        {
            charHolder = CardHolderRegistry.Instance.playerCharacterHolder;
            moveHolder = CardHolderRegistry.Instance.playerMoveHolder;
            diceHolder = CardHolderRegistry.Instance.playerDiceHolder;
            otherCharHolder = CardHolderRegistry.Instance.enemyCharacterHolder;
        }
        else
        {
            charHolder = CardHolderRegistry.Instance.enemyCharacterHolder;
            moveHolder = CardHolderRegistry.Instance.enemyMoveHolder;
            diceHolder = CardHolderRegistry.Instance.enemyDiceHolder;
            otherCharHolder = CardHolderRegistry.Instance.playerCharacterHolder;
        }

        initialised = true;
    }

    public IEnumerator PerformAttack()
    {
        if(!initialised) yield break;

        dice = diceHolder.cards[currentIndex].GetComponent<Dice>();

        if (dice == null || characterData == null)
            yield break;

        int roll = dice.currDiceVal;
        MoveData move = GetMoveByRoll(roll);
        if (move == null)
            yield break;

        Debug.Log($"{characterData.characterName} rolled {roll} and used {move.moveName}");

        if (animator != null)
            animator.Play(move.animation.name);

        yield return new WaitForSeconds(move.hitDelay);

        ApplyMoveEffect(move);

        yield return new WaitForSeconds(move.endDelay);
    }

    MoveData GetMoveByRoll(int roll)
    {
        if (characterData.moveDatas == null || roll <= 0 || roll > characterData.moveDatas.Length)
            return null;
        return characterData.moveDatas[roll - 1];
    }

    void ApplyMoveEffect(MoveData move)
    {
        if (move.moveEffect != null)
        {
            move.moveEffect.Apply(card, charHolder, otherCharHolder, move);
        }
        else
        {
            Debug.LogWarning($"{move.moveName} has no MoveEffect assigned!");
        }
    }

}
