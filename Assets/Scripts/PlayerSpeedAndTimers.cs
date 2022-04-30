using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedAndTimers : MonoBehaviour
{



    internal float walkSpeed = 4f;
    internal float runningSpeed = 6f;
    internal float CrawlingSpeed = 2f;
    internal float SlidingSpeed = 4.5f;


    //----------------------------------Timers
    internal float turnTimer = 0;
    internal float turnTimerSet = 0.1f;

    internal float crawlTimer = 0.15f;
    internal float crawlTimerSet = 0.15f;

    internal float standUpTimer = 0;
    internal float standUpTimerSet = 0.15f;

    internal float slidingTimer;
    internal float slidingTimerSet = 0.8f;

    internal float climbLedgeTimer = 0;
    internal float climbLedgeTimerSet = 0.5f;

    internal float hangTime = 0.1f;
    internal float hangTimeTimer;

    internal float jumpBufferLength = .05f;
    internal float jumpBufferCount;

    internal float slideTransitionTimer;
    internal float slideTransitionTimerSet = 0.5f;





}
