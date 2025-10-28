using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// All dialogue scripts are from Semag Games youtube tutorial

public class Response 
{

    [SerializeField] private string responseText;
    [SerializeField] private DialogueObject dialogueObject;

    public string ResponseText => responseText;
    public DialogueObject DialogueObject => dialogueObject;

}
