using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assigning the buttons to the gamestate manager on start because doing so directly in the editor creates multiple instances of statemanager
/// </summary>

public class GameStateButtonBinder : MonoBehaviour
{
    [System.Serializable]
    public struct StateButton
    {
        public GameStateType state;
        public Button button;
    }

    public StateButton[] buttons;

    void Start()
    {
        foreach (var sb in buttons)
        {
            if (sb.button != null)
            {
                sb.button.onClick.AddListener(() => GameStateManager.Instance.SetState(sb.state));
            }
        }
    }
}
