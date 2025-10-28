using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RouletteSpawner : MonoBehaviour
{
    [Header("Setup")]
    public RouletteTemplate template;
    public float RadiusSize = 3f;
    public float BallSize = 1f;
    public Transform ParentTrans;
    public GameObject RouletteBallPrefab;

    private List<GameObject> spawnedBalls = new List<GameObject>();
    private List<BallType> randomizedOrder = new List<BallType>();

    private RouletteSpinner rouletteSpinner;


    private void Awake()
    {
        rouletteSpinner = GetComponent<RouletteSpinner>();
    }

    void OnEnable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    void OnGameStateChanged(GameStateType newState)
    {
        if (newState == GameStateType.Roulette)
            SpawnRoulette();
        else
            DespawnRouletteKeepOrder(true);
    }

    public void SpawnRoulette()
    {
        // If roulette order does not exist, make new one
        bool rouletteListEmpty = randomizedOrder == null || randomizedOrder.Count == 0;

        if (rouletteListEmpty)
        {
            FillRandomRouletteList();
        }

        PlaceBallsOnRoulette();
    }

    void DespawnRouletteKeepOrder(bool keepOrder)
    {
        foreach (var ball in spawnedBalls)
        {
            if(ball != null)
                Destroy(ball);
        }

        spawnedBalls.Clear();


        if (!keepOrder)
            EmptyRouletteList();
    }

    public void EmptyRouletteList()
    {
        randomizedOrder.Clear();
    }


    void FillRandomRouletteList()
    {
        List<(BallType type, int count)> ballData = new List<(BallType, int)>
        {
            (BallType.Normal, template.Normal),
            (BallType.Enemy, template.Enemy),
            (BallType.Treasure, template.Treasure),
            (BallType.Trap, template.Trap),
            (BallType.Other, template.Other),
            (BallType.Story, template.Story)
        };

        List<BallType> allBalls = new List<BallType>();
        foreach (var (type, count) in ballData)
        {
            for (int i = 0; i < count; i++)
                allBalls.Add(type);
        }

        randomizedOrder = ShuffleList(allBalls);
    }

    void PlaceBallsOnRoulette()
    {
        Vector3 center = ParentTrans != null ? ParentTrans.position : transform.position;
        int totalCount = randomizedOrder.Count;

        for (int i = 0; i < totalCount; i++)
        {
            float angle = 360f / totalCount * i;
            Vector3 pos = center + new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad) * RadiusSize,
                Mathf.Cos(angle * Mathf.Deg2Rad) * RadiusSize,
                0f
            );

            GameObject ball = Instantiate(RouletteBallPrefab, pos, Quaternion.identity, ParentTrans);
            ball.transform.localScale = Vector3.one * BallSize;

            var logic = ball.GetComponent<RouletteBallLogic>();
            if (logic != null)
                logic.Initialize(randomizedOrder[i]);

            spawnedBalls.Add(ball);
        }
    }

    public void StartSpin()
    {
        rouletteSpinner.SpinWheel(spawnedBalls);
    }


    List<T> ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
        return list;
    }
}

public enum BallType
{
    Normal,
    Enemy,
    Treasure,
    Trap,
    Other,
    Story
}
