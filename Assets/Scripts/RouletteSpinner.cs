using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RouletteSpinner : MonoBehaviour
{
    // The list of balls to iterate through (assuming they are children or managed by another script)
    // In a real scenario, you'd likely get this from the RouletteSpawner or a manager.
    [SerializeField]
    private List<GameObject> rouletteBalls = new List<GameObject>();

    [Header("Spin Settings")]
    public float initialInterval = 0.03f;
    public float[] slowdownIntervals = { 0.1f, 0.3f, 0.6f };
    public Vector2 slowdownDurationRange = new Vector2(1f, 2f); // Min and Max time for each stage

    // Events to signal the start and end of the spin
    public event Action OnSpinStarted;
    public event Action<GameObject> OnSpinFinished; // Passes the final selected ball

    private bool isSpinning = false;
    private Coroutine spinCoroutine;
    private int currentIndex = 0;
    // New field to track the ball from the previous iteration
    private int previousIndex = -1;

    /// <summary>
    /// Call this to start the roulette wheel spin.
    /// </summary>
    /// <param name="balls">The list of instantiated roulette balls to spin through.</param>
    public void SpinWheel(List<GameObject> balls)
    {
        if (isSpinning) return;

        rouletteBalls = balls;

        if (rouletteBalls == null || rouletteBalls.Count == 0)
        {
            Debug.LogError("Cannot spin, rouletteBalls list is empty.");
            return;
        }

        // Reset tracking indices when starting a new spin
        currentIndex = 0;
        previousIndex = -1;

        isSpinning = true;
        OnSpinStarted?.Invoke();
        spinCoroutine = StartCoroutine(SpinRoutine());
    }

    /// <summary>
    /// The coroutine that handles the multi-stage slowdown spin.
    /// </summary>
    IEnumerator SpinRoutine()
    {
        float currentInterval = initialInterval;
        float timeElapsedInStage = 0f;
        float targetDurationForStage = UnityEngine.Random.Range(slowdownDurationRange.x, slowdownDurationRange.y);

        // --- Stage 0: Initial Fast Spin ---
        while (timeElapsedInStage < targetDurationForStage)
        {
            HighlightCurrentBall(currentIndex);

            // Store the current index before moving
            previousIndex = currentIndex;

            // Move to the next index
            currentIndex = (currentIndex + 1) % rouletteBalls.Count;
            // Debug.Log(currentIndex);

            yield return new WaitForSeconds(currentInterval);
            timeElapsedInStage += currentInterval;
        }

        // --- Slowdown Stages ---
        for (int stage = 0; stage < slowdownIntervals.Length; stage++)
        {
            currentInterval = slowdownIntervals[stage];
            timeElapsedInStage = 0f;
            targetDurationForStage = UnityEngine.Random.Range(slowdownDurationRange.x, slowdownDurationRange.y);

            while (timeElapsedInStage < targetDurationForStage)
            {
                HighlightCurrentBall(currentIndex);

                // Store the current index before moving
                previousIndex = currentIndex;

                // Move to the next index
                currentIndex = (currentIndex + 1) % rouletteBalls.Count;
                // Debug.Log(currentIndex);

                yield return new WaitForSeconds(currentInterval);
                timeElapsedInStage += currentInterval;
            }
        }

        // --- Final Stop ---
        // Deselect the last ball highlighted during the spin loop before the final selection
        DeselectBall(previousIndex);

        // Final selection highlight
        HighlightCurrentBall(currentIndex, true);
        GameObject finalBall = rouletteBalls[currentIndex];

        isSpinning = false;
        Debug.Log($"Roulette stopped on ball index {currentIndex}.");

        // Notify subscribers (like the RouletteSpawner or a Game Manager)
        OnSpinFinished?.Invoke(finalBall);
    }

    /// <summary>
    /// Attempts to deselect a ball at a specific index.
    /// </summary>
    void DeselectBall(int index)
    {
        // Only deselect if there was a previous ball and it's a valid index
        if (index >= 0 && index < rouletteBalls.Count)
        {
            var logic = rouletteBalls[index].GetComponent<RouletteBallLogic>();
            logic?.DeselectBall();
        }
    }


    /// <summary>
    /// Handles the visual feedback for the currently selected ball.
    /// </summary>
    /// <param name="index">The index of the ball to highlight.</param>
    /// <param name="isFinal">True if this is the final selection (not used for this simple scale change, but kept for future use).</param>
    void HighlightCurrentBall(int index, bool isFinal = false)
    {
        // 1. Deselect the previously highlighted ball
        DeselectBall(previousIndex);

        // 2. Select the current ball
        if (index >= 0 && index < rouletteBalls.Count)
        {
            var currentBallLogic = rouletteBalls[index].GetComponent<RouletteBallLogic>();
            currentBallLogic?.SelectBall();
        }
    }
}