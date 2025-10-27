using System.Collections.Generic;
using UnityEngine;

public class RouletteSpawner : MonoBehaviour
{
    [Header("Setup")]
    public RouletteTemplate template;
    public float RadiusSize = 3f;
    public Transform ParentTrans;
    public GameObject RouletteBallPrefab;

    private List<GameObject> spawnedBalls = new List<GameObject>();
    private List<Color> randomizedOrder = new List<Color>();

    void Start()
    {
        SpawnRoulette();
    }

    void SpawnRoulette()
    {
        foreach (var go in spawnedBalls)
            Destroy(go);
        spawnedBalls.Clear();

        List<(Color color, int count)> ballData = new List<(Color, int)>
        {
            (Color.blue, template.Normal),
            (new Color(1f, 0.5f, 0f), template.Enemy),
            (Color.yellow, template.Treasure),
            (Color.red, template.Trap),
            (Color.black, template.Other),
            (Color.green, template.Story)
        };

        List<Color> allBalls = new List<Color>();
        foreach (var (color, count) in ballData)
        {
            for (int i = 0; i < count; i++)
                allBalls.Add(color);
        }

        randomizedOrder = ShuffleList(allBalls);

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

            GameObject ball = Instantiate(RouletteBallPrefab, pos, Quaternion.identity);
            ball.transform.SetParent(ParentTrans, true);

            var renderer = ball.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = randomizedOrder[i];

            spawnedBalls.Add(ball);
        }
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
