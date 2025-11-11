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
        if (hpSlider != null)
            hpSlider.value = currentHealth;
    }


    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthSlider();

        if (currentHealth <= 0)
            Kill();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthSlider();
    }


    public void Kill()
    {

    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public void InitialiseHealthSlider()
    {
        card = GetComponent<Card>();
        if (card == null) return;

        characterData = card.characterData;
        if (characterData == null) return;

        cardVisual = card.cardVisual;
        if (cardVisual == null) return;

        hpSlider = cardVisual.GetComponentInChildren<Slider>();
        if (hpSlider == null) return;
        
        // On first run
        if (characterData.maxHp == 0)
        {
            maxHealth = characterData.startingMaxHP;
            currentHealth = maxHealth;
        }
        else
        {
            maxHealth = characterData.maxHp;
        }

        if (healOnCombatStart)
            currentHealth = maxHealth;

        hpSlider.maxValue = maxHealth;
        hpSlider.value = currentHealth;
        initialised = true;
    }
}
