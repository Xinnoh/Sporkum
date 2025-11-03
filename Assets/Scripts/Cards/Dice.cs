using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    public int minDiceVal = 1;
    public int maxDiceVal = 6;

    private Card card;
    public Sprite[] diceSprites;

    public int currDiceVal;


    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<Card>();

        RerollValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RerollValue()
    {
        // +1 because doesn't roll the max otherwise
        currDiceVal = Random.Range((int)minDiceVal, (int)maxDiceVal + 1);

        card.cardVisual.UpdateSprite(diceSprites[currDiceVal - 1]);
    }

}
