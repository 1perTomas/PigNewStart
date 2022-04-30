using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{

    public string dialogue;
    public string[] dialogueLines;



    internal bool introduced = false; // test
    internal bool annoyed = false;
    [SerializeField]
    internal SignDialogues signDialogue; // test
    private DialogueManager dialogueManager;
    private bool playerInRange;



    private Camera cam;
    Vector2 positionOnScreen;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        dialogueManager = FindObjectOfType<DialogueManager>();

    }

    // Update is called once per frame
    void Update()
    {
        Interact();
        TextBoxOverObject();
    }

    private void FixedUpdate()
    {
        if (dialogueManager.typewriterEffect.canAdvance)
        {
            dialogueManager.contText.text = "E to continue";
        }
        else
        {
            dialogueManager.contText.text = " ";
        }
    }


    private void Interact()
    {

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {

            if (!dialogueManager.dialogueActive)
            {

                dialogueManager.dialogueLines = signDialogue.stringInUse; // test
                //dialogueManager.dialogueLines = dialogueLines;
                dialogueManager.currentLine = 0;
                dialogueManager.ShowDialogue(gameObject.name);

                //dialogueManager. 
                //dialogueManager.ShowBox(gameObject.name, dialogue);
            }

            else if (dialogueManager.dialogueActive && dialogueManager.currentLine < dialogueManager.dialogueLines.Length - 1 && dialogueManager.typewriterEffect.canAdvance)
            {
                dialogueManager.currentLine++;
                dialogueManager.ShowDialogue(gameObject.name);

            }

            else if (dialogueManager.dialogueActive && dialogueManager.currentLine == dialogueManager.dialogueLines.Length - 1 && dialogueManager.typewriterEffect.canAdvance)
            {
                if (introduced)
                {
                    annoyed = true;
                }

                dialogueManager.CloseBox();
                dialogueManager.currentLine = 0;
                introduced = true; // test

            }
        }

    }

    private void TextBoxOverObject()
    {
        positionOnScreen = (cam.WorldToScreenPoint(gameObject.transform.position));
        dialogueManager.dialogueBox.transform.position = new Vector2(positionOnScreen.x, positionOnScreen.y + 150f);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.dialogueHolder = gameObject.GetComponent<DialogueHolder>();
            playerInRange = true;
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.dialogueHolder = null;
            dialogueManager.CloseBox();
            playerInRange = false;
            Debug.Log("Player left range");
        }
    }
}
