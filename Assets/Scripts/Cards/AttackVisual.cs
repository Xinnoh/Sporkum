using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AttackVisual : MonoBehaviour
{
    [Header("Setup")]
    public List<Image> moveImages;
    public List<TMP_Text> moveTexts;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color normalOutlineColor = Color.gray;
    public Color selectedOutlineColor = Color.yellow;

    [Header("Height")]
    public float normalHeight = 24f;
    public float selectedHeight = 30f;

    [Header("Outline Widths")]
    public float normalOutlineSize = 2f;
    public float selectedOutlineSize = 4f;

    [Header("Tween Settings")]
    public float tweenDuration = 0.2f;
    public Ease tweenEase = Ease.OutQuad;

    private int selectedIndex = -1;

    public void UpdateMove(int index)
    {
        index -= 1;
        if (index < 0 || index >= moveImages.Count)
            return;

        for (int i = 0; i < moveImages.Count; i++)
        {
            var img = moveImages[i];
            if (img == null) continue;

            bool isSelected = i == index;

            // Tween color
            img.DOColor(isSelected ? selectedColor : normalColor, tweenDuration);

            // Tween height
            var rect = img.rectTransform;
            float targetHeight = isSelected ? selectedHeight : normalHeight;
            DOTween.To(() => rect.sizeDelta, x => rect.sizeDelta = x, new Vector2(rect.sizeDelta.x, targetHeight), tweenDuration)
                   .SetEase(tweenEase);

            // Outline
            var outline = img.GetComponent<Outline>();
            if (outline != null)
            {
                Color targetOutlineColor = isSelected ? selectedOutlineColor : normalOutlineColor;
                float targetOutlineSize = isSelected ? selectedOutlineSize : normalOutlineSize;

                DOTween.To(() => outline.effectColor, c => outline.effectColor = c, targetOutlineColor, tweenDuration)
                       .SetEase(tweenEase);
                DOTween.To(() => outline.effectDistance, d => outline.effectDistance = d, new Vector2(targetOutlineSize, targetOutlineSize), tweenDuration)
                       .SetEase(tweenEase);
            }

            // Bold text
            if (i < moveTexts.Count && moveTexts[i] != null)
            {
                moveTexts[i].fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;
            }
        }

        selectedIndex = index;
    }
}
