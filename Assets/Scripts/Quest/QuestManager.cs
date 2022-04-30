using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public QuestObject[] quests;
    public bool[] questCompleted;

    public DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        questCompleted = new bool[quests.Length]; //has to have as many slots as there are quests
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowQuestText(string questText)
    {
        dialogueManager.dialogueLines = new string[1];
        dialogueManager.dialogueLines[0] = questText;

        dialogueManager.currentLine = 0;
        dialogueManager.ShowDialogue("Quest");
    }
}
