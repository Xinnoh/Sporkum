using System;
using UnityEngine;

public enum GameStateType
{
    Menu,
    Roulette,
    Dungeon,
    Combat,
    Training,
    Neutral,
    Shop,
    Sacrifice
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameStateType CurrentState { get; private set; }

    public event Action<GameStateType> OnGameStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        CurrentState = GameStateType.Menu;
    }

    public void SetState(GameStateType newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        Debug.Log("Game state changed to: " + newState);
        OnGameStateChanged?.Invoke(newState);
    }

    public void SetMenu() => SetState(GameStateType.Menu);
    public void SetRoulette() => SetState(GameStateType.Roulette);
    public void SetDungeon() => SetState(GameStateType.Dungeon);
    public void SetCombat() => SetState(GameStateType.Combat);
}
