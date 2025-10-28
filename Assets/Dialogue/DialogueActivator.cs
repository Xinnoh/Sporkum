using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{

    [SerializeField] private DialogueObject dialogueObject;

    public void Interact(Player player)
    {
        player.DialogueUI.ShowDialogue(dialogueObject);
    }



}
