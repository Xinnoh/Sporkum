using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueActivator : MonoBehaviour,  IInteractable
{

    [SerializeField] private DialogueObject dialogueObject;

    private DialogueUI dialogueUI;
    private DialogueUI DialogueUI => dialogueUI;
    private Player player;

    private void Start()
    {
        GameObject uiCanvas = GameObject.FindWithTag("UICanvas");
        if (uiCanvas != null)
            dialogueUI = uiCanvas.GetComponent<DialogueUI>();

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) 
            player = playerObject.GetComponent<Player>();


        
    }

    private void OnMouseDown()
    {
        player.Interactable = this;
    }




    public void Interact(Player player)
    {
        player.DialogueUI.ShowDialogue(dialogueObject);
        player.Interactable = null;
    }



}
