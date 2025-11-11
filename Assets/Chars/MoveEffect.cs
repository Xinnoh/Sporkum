using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveEffect : MonoBehaviour
{
    public abstract void Apply(Card userCard, HorizontalCardHolder charHolder, HorizontalCardHolder otherCharHolder, MoveData move);

}
