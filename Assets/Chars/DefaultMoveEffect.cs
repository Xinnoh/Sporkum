using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/MoveEffect/Default")]
public class DefaultMoveEffect : MoveEffect
{
    public override void Apply(Card userCard, HorizontalCardHolder charHolder, HorizontalCardHolder otherCharHolder, MoveData move)
    {
        var targets = GetTargets(userCard, charHolder, otherCharHolder, move);

        foreach (var target in targets)
            ApplyEffect(userCard, target, move);
    }

    private List<Card> GetTargets(Card userCard, HorizontalCardHolder charHolder, HorizontalCardHolder otherCharHolder, MoveData move)
    {
        var targets = new List<Card>();

        switch (move.targetType)
        {
            case MoveData.MoveTarget.SingleEnemy:
                var validTargets = otherCharHolder.cards.Where(c => c != null);
                Card singleTarget = userCard.isEnemy
                    ? validTargets.OrderByDescending(c => c.ParentIndex()).FirstOrDefault()
                    : validTargets.OrderBy(c => c.ParentIndex()).FirstOrDefault();

                if (singleTarget != null)
                    targets.Add(singleTarget);
                break;

            case MoveData.MoveTarget.AllEnemies:
                targets.AddRange(otherCharHolder.cards.Where(c => c != null));
                break;

            case MoveData.MoveTarget.AllAllies:
                targets.AddRange(charHolder.cards.Where(c => c != null));
                break;

            case MoveData.MoveTarget.Self:
                targets.Add(userCard);
                break;
        }

        return targets;
    }

    private void ApplyEffect(Card userCard, Card target, MoveData move)
    {
        // this is where you define what the move *does*
        var health = target.GetComponent<HealthHandler>();

        switch (move.effectType)
        {
            case MoveData.EffectType.Damage:
                health?.TakeDamage(move.power);
                break;

            case MoveData.EffectType.Heal:
                health?.Heal(move.power);
                break;

            case MoveData.EffectType.Buff:
                //target.GetComponent<StatusManager>()?.ApplyBuff(move);
                break;

            case MoveData.EffectType.Debuff:
                //target.GetComponent<StatusManager>()?.ApplyDebuff(move);
                break;

        }

        Debug.Log($"{move.moveName} applied {move.effectType} ({move.power}) to {target.name}");
    }
}
