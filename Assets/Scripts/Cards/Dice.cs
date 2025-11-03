using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    public int minDiceVal = 1;
    public int maxDiceVal = 6;

    private Card card;
    public Sprite[] diceSprites;

    private int diceVal { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<Card>();
        diceVal = Random.Range(minDiceVal, maxDiceVal);

        RerollValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RerollValue()
    {
        diceVal = Random.Range(minDiceVal, maxDiceVal);

        card.cardVisual.UpdateSprite(diceSprites[diceVal - 1]);

    }

}
