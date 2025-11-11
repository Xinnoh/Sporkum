using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Move")]


public class MoveData : ScriptableObject
{
    public string moveName;
    [TextArea] public string description;

    public int power;
    public AnimationClip animation;

    public float hitDelay, endDelay;

    public MoveTarget targetType;

    public enum MoveTarget
    {
        SingleEnemy,
        AllEnemies,
        Self,
        AllAllies,
        SpecialTarget
    }

    public EffectType effectType;
    public enum EffectType
    {
        Damage,
        Heal,
        Buff,
        Debuff
    }
    public MoveEffect moveEffect;
}
