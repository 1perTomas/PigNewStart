using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    internal DialogueHolder dialogueHolder;

    //[SerializeField] private DialogueObject testDialogue;

    internal TypewriterEffect typewriterEffect;

    public GameObject dialogueBox;
    public TMP_Text objectName;
    public TMP_Text dText;
    public TMP_Text contText;

    public string[] dialogueLines;
    public int currentLine;
    private RectTransform boxSize;
    private RectTransform textBoxSize;
    


    public bool dialogueActive = false;

   

    // Start is called before the first frame update
    void Start()
    {
        //continueButton.GetComponent<TextMeshProUGUI>().text = "false";
       
        textBoxSize = dText.GetComponent<RectTransform>();
        boxSize = dialogueBox.GetComponent<RectTransform>();
        typewriterEffect = GetComponent<TypewriterEffect>();
       

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive)
        {
           // playerController.playerState.isInteracting = true;
            playerController.playerMovement.canMove = false;
        }
        else
        {
           // playerController.playerState.isInteracting = false;
           // playerController.playerMovement.canMove = true;
        }

    }

    public void ShowBox(string name, string dialogue)
    {
        //playerController.playerMovement.isInteracting = true;
        //playerController.playerMovement.canMove = false;
        
        dialogueActive = true;
        dialogueBox.SetActive(true);
        dText.text = dialogue;
        objectName.text = name;
    }

    public void ShowDialogue(string name)
    {
        dialogueActive = true;
        dialogueBox.SetActive(true);
        //continueButtonObj.SetActive(false);
       

        dText.text = dialogueLines[currentLine];
        if (dText.text.Length * 5 < 80)
        {
            textBoxSize.sizeDelta = new Vector2(80, 25);
            boxSize.sizeDelta = new Vector2(80, 25); // adjusts box size according to contents
        }
        else
        {
            textBoxSize.sizeDelta = new Vector2(dText.text.Length * 5, 15);
            boxSize.sizeDelta = new Vector2(dText.text.Length * 5, 25); // adjusts box size according to contents
        }
        
        typewriterEffect.Run(dText.text, dText,contText);
        objectName.text = name;
       
        

    }

    public void CloseBox()
    {
        //playerController.playerMovement.isInteracting = false;
        //playerController.playerMovement.canMove = true;
        dialogueActive = false;
        dialogueBox.SetActive(false);
    }

    public void TextDivision()
    {

    }


}
