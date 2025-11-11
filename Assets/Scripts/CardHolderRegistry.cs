using UnityEngine;

public class CardHolderRegistry : MonoBehaviour
{
    public static CardHolderRegistry Instance { get; private set; }

    [Header("Assign in Inspector")]
    public HorizontalCardHolder playerCharacterHolder;
    public HorizontalCardHolder enemyCharacterHolder;
    public HorizontalCardHolder playerMoveHolder;
    public HorizontalCardHolder enemyMoveHolder;
    public HorizontalCardHolder playerDiceHolder;
    public HorizontalCardHolder enemyDiceHolder;

    [HideInInspector] public HorizontalCardHolder[] playerHolders;
    [HideInInspector] public HorizontalCardHolder[] enemyHolders;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerHolders = new HorizontalCardHolder[] { playerCharacterHolder, playerMoveHolder, playerDiceHolder };
        enemyHolders = new HorizontalCardHolder[] { enemyCharacterHolder, enemyMoveHolder, enemyDiceHolder };

    }
}
