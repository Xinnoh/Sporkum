using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// All dialogue scripts are from Semag Games youtube tutorial
public class TyperwriterEffect : MonoBehaviour
{

    [SerializeField] private float typewriterSpeed = 30f;


    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(TypeText(textToType, textLabel));
    }


    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {

        textLabel.text = string.Empty;

        float t = 0f;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * typewriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        textLabel.text = textToType;

    }


}
