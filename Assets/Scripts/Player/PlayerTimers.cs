using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimers : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    //----------------------------------Timers
    internal float turnTimer = 0;
    internal float turnTimerSet = 0.1f;

    internal float crawlTimer;  //used in animation manager
    internal float crawlTimerSet = 0.15f;

    internal float standUpTimer = 0;
    internal float standUpTimerSet = 0.15f;

   // internal float slidingTimer;
   // internal float slidingTimerSet = 0.8f;

    internal float slideTransitionTimer;
    internal float slideTransitionTimerSet = 0.5f;

    internal float climbLedgeTimer = 0;
    public float climbLedgeTimerSet = 0.3f;

    internal float hangTime = 0.1f;
    internal float hangTimeTimer;

    internal float jumpBufferLength = .05f;
    internal float jumpBufferCount;

    internal float wallJumpTimer;
    internal float wallJumpTimerSet = 0.05f;

    internal void WallJumpBugFix()
    {
        if (playerController.playerState.currentState == PlayerState.CharacterMovement.WallJump)
        {
            wallJumpTimer = wallJumpTimerSet;
        }

        else
        {
            if (wallJumpTimer > 0)
            {
                wallJumpTimer -= Time.deltaTime;
            }
            else if (wallJumpTimer < 0)
            {
                wallJumpTimer = 0;
            }
        }

    }

    internal void PositionTransition()
    {
        if (playerController.playerState.isStanding || playerController.playerState.currentState == PlayerState.CharacterMovement.Sliding)
        {
            crawlTimer = crawlTimerSet;

            if (standUpTimer > 0)
            {
                standUpTimer -= Time.deltaTime;
            }

            else if (standUpTimer < 0)
            {
                standUpTimer = 0;
            }
        }

        else
        {
            standUpTimer = standUpTimerSet;

            if (crawlTimer > 0)
            {
                crawlTimer -= Time.deltaTime;
            }

            else if (crawlTimer < 0)
            {
                crawlTimer = 0;
            }
        }
    }
}



