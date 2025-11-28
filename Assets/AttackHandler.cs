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
    private int currentAttackIndex;

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


        if (characterData == null)
            yield break;


        MoveData move = GetMoveByRoll(currentAttackIndex);
        if (move == null)
            yield break;

        if (card.isEnemy)
        {
            Debug.Log($"Enemy {characterData.characterName} in slot {card.ParentIndex() + 1} used {move.moveName} from move {currentAttackIndex}"); 
        }
        else
        {
            Debug.Log($"Player's {characterData.characterName} in slot {card.ParentIndex() + 1} used {move.moveName} from move {currentAttackIndex}");
        }


        if (animator != null)
            animator.Play(move.animation.name);



        yield return new WaitForSeconds(move.hitDelay);

        ApplyMoveEffect(move);

        yield return new WaitForSeconds(move.endDelay);

        yield return new WaitForSeconds(1f);
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

    public void UpdateAttackIndex(int newIndex)
    {
        currentAttackIndex = newIndex;
    }

}
