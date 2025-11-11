using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Character")]


public class CharacterData : ScriptableObject
{
    public string characterName;
    public string description;
    public Sprite sprite;
    public bool flipSprite;


    public int startingMaxHP, startingAttack;

    public MoveData[] moveDatas;

    [HideInInspector] public int maxHp, currentHp, attack, currentAttack;


}
