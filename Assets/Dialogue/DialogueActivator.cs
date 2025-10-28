using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// All dialogue scripts are from Semag Games youtube tutorial

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

    // If interacting with something a second time, allows a different text to display
    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }



    private void OnMouseDown()
    {
        player.Interactable = this;
    }




    public void Interact(Player player)
    {

        foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if(responseEvents.DialogueObject == dialogueObject)
            {
                player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }


        player.DialogueUI.ShowDialogue(dialogueObject);
        player.Interactable = null;
    }



}
