using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Slider hpSlider;
    private int maxHealth;
    private int currentHealth = 3;
    private CharacterData characterData;
    private Card card;

    [Header("Settings idk")]
    public bool healOnCombatStart;

    private CardVisual cardVisual;

    private bool initialised;
    private CombatManager combatManager;
    private bool isEnemy;


    void Start()
    {
        combatManager = GameObject.FindWithTag("CombatManager").GetComponent<CombatManager>();

    }

    void Update()
    {
        if (!initialised) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentHealth += -1;
            UpdateHealthSlider();
        }

        if(currentHealth <= 0)
        {

        }
    }

    public void UpdateHealthSlider()
    {
        hpSlider.value = currentHealth;
    }


    public void TakeDamage()
    {

    }

    public void Heal()
    {

    }

    public void Kill()
    {

    }


    public void InitialiseHealthSlider()
    {
        card = GetComponent<Card>();

        characterData = card.characterData;


        maxHealth = characterData.hp;
        cardVisual = card.cardVisual;

        hpSlider = cardVisual.GetComponentInChildren<Slider>();


        if (healOnCombatStart)
        {
            currentHealth = maxHealth;
        }

        hpSlider.maxValue = maxHealth;
        hpSlider.value = currentHealth;
        initialised = true;
    }
}
