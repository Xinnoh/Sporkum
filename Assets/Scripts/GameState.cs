using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateType
{
    Menu,
    Roulette,
    Dungeon,
    Combat
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameStateType CurrentState { get; private set; }

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
        CurrentState = newState;
        Debug.Log("Game state changed to: " + newState);
    }

    public void SetMenu() => SetState(GameStateType.Menu);
    public void SetRoulette() => SetState(GameStateType.Roulette);
    public void SetDungeon() => SetState(GameStateType.Dungeon);
    public void SetCombat() => SetState(GameStateType.Combat);
}
