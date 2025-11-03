using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private DiceManager diceManager;

    [SerializeField] HorizontalCardHolder[] horizontalCardHolders;

    // Start is called before the first frame update
    void Start()
    {
        diceManager = GetComponent<DiceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCombat();
        }
    }


    public void StartCombat()
    {
        foreach (var holder in horizontalCardHolders)
        {
            holder.StartCombat();
        }



    }
}
