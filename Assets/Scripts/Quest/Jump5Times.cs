using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump5Times : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    internal int jumpCount = 0;

    bool isJumping;
    internal bool isQuestActive;
    internal bool conditionFulfilled = false;


    void Update()
    {
        if (isQuestActive)
        {

            if (jumpCount < 5)
            {
                CheckForQuest();
            }
        }
    }

    private void CheckForQuest()
    {

        if (player.GetComponent<PlayerController>().playerInput.isJumpTapped && player.GetComponent<PlayerController>().playerState.canJump)
        {

            jumpCount += 1;
        }

        if (jumpCount >= 5)
        {
            conditionFulfilled = true;
           // if 
           // print("Quest Complete");
        }
    }

}
