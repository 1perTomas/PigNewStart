using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedList : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;

    public float walkSpeed = 3.5f;
    public float runningSpeed = 6f;
    public float crawlingSpeed = 2f;
    public float slidingSpeed = 4.5f;
    public float wallSlideSpeed = 0.45f;

    internal void FlipSpeedValues()
    {
        walkSpeed *= -1;
        runningSpeed *= -1;
        crawlingSpeed *= -1;
        slidingSpeed *= -1;
    }

}
