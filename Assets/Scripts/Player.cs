using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable {  get; set; }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (dialogueUI.IsOpen) return;

        if (Input.GetMouseButtonDown(0))
        {
            Interactable?.Interact(this);
        }
    }

}

