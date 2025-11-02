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

    public int hp;
    public int attack;

    public MoveData move1, move2, move3,move4,move5,move6;

}
