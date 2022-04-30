using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{

    private QuestManager questManager;

    public int questNumber;

    public bool startQuest;
    public bool endQuest;
    public bool initializeQuest = false;


    // Start is called before the first frame update
    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered the quest area");
            initializeQuest = true;


           // if (!questManager.questCompleted[questNumber])
           // {
           //     if(startQuest && !questManager.quests[questNumber].gameObject.activeSelf)
           //     {
           //         questManager.quests[questNumber].gameObject.SetActive(true);
           //         questManager.quests[questNumber].StartQuest();
           //
           //     }
           //
           //
           //
           // }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player exited the quest area");
        }
    }


}
