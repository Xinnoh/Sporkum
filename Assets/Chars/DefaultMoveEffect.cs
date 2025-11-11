using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Character/MoveEffect/Default")]
public class DefaultMoveEffect : MoveEffect
{
    public override void Apply(Card userCard, HorizontalCardHolder charHolder, HorizontalCardHolder otherCharHolder, MoveData move)
    {
        if (move.targetType == MoveData.MoveTarget.SingleEnemy)
        {
            Card target = userCard.isEnemy
                ? otherCharHolder.cards.Where(c => c != null).OrderByDescending(c => c.ParentIndex()).FirstOrDefault()
                : otherCharHolder.cards.Where(c => c != null).OrderBy(c => c.ParentIndex()).FirstOrDefault();

            if (target != null)
                target.GetComponent<HealthManager>()?.TakeDamage(move.power);
        }
        else if (move.targetType == MoveData.MoveTarget.AllEnemies)
        {
            foreach (var c in otherCharHolder.cards)
                c?.GetComponent<HealthManager>()?.TakeDamage(move.power);
        }
        else if (move.targetType == MoveData.MoveTarget.AllAllies)
        {
            foreach (var c in charHolder.cards)
                c?.GetComponent<HealthManager>()?.TakeDamage(move.power);
        }
        else if (move.targetType == MoveData.MoveTarget.Self)
        {
            userCard.GetComponent<HealthManager>()?.TakeDamage(move.power);
        }

        Debug.Log($"{move.moveName} applied for {move.power} damage");
    }
}
