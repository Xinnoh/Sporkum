using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Move")]


public class MoveData : ScriptableObject
{
    public string moveName;
    [TextArea] public string description;

    public int power;
    public MoveTarget targetType;
    public AnimationClip animation;

    public float hitDelay, endDelay;

    public MoveEffect moveEffect;
    public enum MoveTarget
    {
        SingleEnemy,
        AllEnemies,
        Self,
        AllAllies,
        Other
    }
}
