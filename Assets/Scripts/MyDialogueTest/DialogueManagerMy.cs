using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManagerMy : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Jump5Times questConditions;

   // [SerializeField]
   // private GameObject questThing;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public string dialogue;
    public bool playerInRange;
    Vector2 positionOnScreen;




    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }


    private void Update()
    {
        SignInteraction();


    }

    private void SignInteraction() // add if (has quest)
    {
        if (playerController.playerInput.isInteractTapped && playerInRange)
        {
            if (dialogueBox.activeInHierarchy)
            {
                questConditions.isQuestActive = true;
                dialogueBox.SetActive(false);
                playerController.playerMovement.canMove = true;
                playerController.playerMovement.isInteracting = false;

                
            }

            else
            {
                positionOnScreen = (cam.WorldToScreenPoint(gameObject.transform.position));
                dialogueBox.transform.position = new Vector2(positionOnScreen.x, positionOnScreen.y+150f); 
                dialogueBox.SetActive(true);
                dialogueText.text = dialogue;
                playerController.playerMovement.isInteracting = true;
                playerController.playerMovement.canMove = false;

                if (questConditions.conditionFulfilled)
                {
                    print("Quest Complete");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueBox.SetActive(false);
            Debug.Log("Player left range");
        }
    }

}
