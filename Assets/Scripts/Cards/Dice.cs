using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    private int minDiceVal = 1;
    private int maxDiceVal = 6;

    private Card card;
    public Sprite[] diceSprites;

    private CharacterData characterData;

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

    public void SetMaxDieValue(CharacterData externalCharacterData)
    {
        characterData = externalCharacterData;

        if (characterData != null && characterData.moveDatas != null)
        {
            maxDiceVal = characterData.moveDatas.Length;
        }
        else
        {
            Debug.LogWarning("No move found");
            maxDiceVal = 10;
        }

        RerollValue();
    }

    public void RerollValue()
    {
        // +1 because doesn't roll the max otherwise
        currDiceVal = Random.Range((int)minDiceVal, (int)maxDiceVal + 1);

        // This works and I have no idea why
        if (card != null && card.cardVisual != null && diceSprites.Length >= currDiceVal)
            card.cardVisual.UpdateSprite(diceSprites[currDiceVal - 1]);
    }

}
