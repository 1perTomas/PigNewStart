using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignDialogues : MonoBehaviour
{
    [SerializeField]
    internal DialogueHolder dialogueHolder;

    public string[] stringInUse;
    public string[] firstEncounter = new string[] { "Hey there, how you doing?", "What can I do you for?" };
    public string[] otherEncounter = new string[] { "We already talked.", "Can't help you anymore." };
    public string[] hoe = new string[] { "...", "...bitch." };


    // Start is called before the first frame update
    void Start()
    {
       // dialogueHolder.GetComponent<DialogueHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        WhatDialogue();
    }

    private void WhatDialogue()
    {

        if (!dialogueHolder.introduced)
        {
            stringInUse = firstEncounter;
        }


        else if (dialogueHolder.introduced && !dialogueHolder.annoyed)
        {
            stringInUse = otherEncounter;

        }

        else
        {
            stringInUse = hoe;
        }


    }


}
