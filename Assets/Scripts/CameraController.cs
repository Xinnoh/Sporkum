using UnityEngine;

/// <summary>
/// Moves and rotates the camera to different preset positions depending on the current game state.
/// Subscribes to GameStateManager to automatically update when the state changes.
/// </summary>
public class CameraStateController : MonoBehaviour
{
    [Header("Camera Positions")]
    public Transform MenuCamPos;
    public Transform RouletteCamPos;
    public Transform DungeonCamPos;
    public Transform CombatCamPos;

    public bool moveCamera;

    [Header("Movement Settings")]
    public float MoveSpeed = 5f;

    private Transform targetPos;
    private Transform lastTarget;

    void Start()
    {
        if (GameStateManager.Instance == null)
            return;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        // Initialize camera position based on current state
        OnGameStateChanged(GameStateManager.Instance.CurrentState);
    }

    void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    void Update()
    {
        if (targetPos == null || !moveCamera)
            return;

        transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime * MoveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetPos.rotation, Time.deltaTime * MoveSpeed);
    }

    /// <summary>
    /// Called whenever the GameStateManager changes state.
    /// Sets the camera's target position.
    /// </summary>
    void OnGameStateChanged(GameStateType newState)
    {
        if (!moveCamera) return;

        switch (newState)
        {
            case GameStateType.Menu: targetPos = MenuCamPos; break;
            case GameStateType.Roulette: targetPos = RouletteCamPos; break;
            case GameStateType.Dungeon: targetPos = DungeonCamPos; break;
            case GameStateType.Combat: targetPos = CombatCamPos; break;
            default: targetPos = null; break;
        }

        if (targetPos == null)
            return;

        transform.position = targetPos.position;
        transform.rotation = targetPos.rotation;
        lastTarget = targetPos;
    }
}
