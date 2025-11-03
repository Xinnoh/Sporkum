using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{


        public void RerollDice()
        {
            foreach (var dice in GameObject.FindGameObjectsWithTag("Dice"))
                dice.GetComponent<Dice>().RerollValue();
    }



}
