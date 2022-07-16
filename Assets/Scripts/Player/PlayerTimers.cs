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

    internal float crawlTimer = 0;  //used in animation manager
    internal float crawlTimerSet = 0.15f;

    internal float standUpTimer = 0;
    internal float standUpTimerSet = 0.15f;

    internal float slidingTimer;
    internal float slidingTimerSet = 0.8f;

    internal float slideTransitionTimer;
    internal float slideTransitionTimerSet = 0.5f;

    internal float climbLedgeTimer = 0;
    internal float climbLedgeTimerSet = 0.3f;

    internal float hangTime = 0.1f;
    internal float hangTimeTimer;

    internal float jumpBufferLength = .05f;
    internal float jumpBufferCount;

    internal void RunTimer (float timer)
    {
        timer += Time.deltaTime;
    }

    internal void ResetTimer(float timer)
    {
        timer = 0;
    }
}
