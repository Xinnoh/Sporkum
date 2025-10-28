using UnityEngine;

public class RouletteBallLogic : MonoBehaviour
{
    public BallType Type;
    private SpriteRenderer spriteRenderer;
    public bool isSelected;

    // Define the scale values for clarity
    private const float SelectedScale = 0.5f;
    private const float DeselectedScale = 0.4f;

    public void Initialize(BallType type)
    {
        Type = type;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial color based on the type
        switch (type)
        {
            case BallType.Normal: spriteRenderer.color = Color.blue; break;
            case BallType.Enemy: spriteRenderer.color = new Color(1f, 0.5f, 0f); break;
            case BallType.Treasure: spriteRenderer.color = Color.yellow; break;
            case BallType.Trap: spriteRenderer.color = Color.red; break;
            case BallType.Other: spriteRenderer.color = Color.black; break;
            case BallType.Story: spriteRenderer.color = Color.green; break;
        }

        // Set the initial scale to the deselected size
        transform.localScale = Vector3.one * DeselectedScale;
    }

    public void SelectBall()
    {
        if (isSelected) return;

        isSelected = true;
        transform.localScale = Vector3.one * SelectedScale;
    }

    public void DeselectBall()
    {
        if (!isSelected) return;

        isSelected = false;
        transform.localScale = Vector3.one * DeselectedScale;
    }

    public void ExecuteBall()
    {
        // Add the logic for what happens when this BallType is chosen
        Debug.Log($"Executing action for ball type: {Type}");
    }
}