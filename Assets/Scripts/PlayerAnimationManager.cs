using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    //Animation states
    //Right side


    const string PLAYER_IDLE = "Idle";
    const string PLAYER_WALK_R = "WalkRight";
    const string PLAYER_WALLSLIDE_R = "WallSlideRight";
    const string PLAYER_RUNNING_R = "RunRight";
    const string PLAYER_GOPRONE_R = "GoProneRight";
    const string PLAYER_CRAWLING_R = "CrawlingRight";
    const string PLAYER_STANDUP_R = "StandUpRight";
    const string PLAYER_SLIDE_R = "SlideRight";
    const string PLAYER_FALL_R = "FallRight";
    const string PLAYER_JUMP_R = "JumpUpRight";
    const string PLAYER_JUMPSLOW_R = "JumpUpSlowRight";
    const string PLAYER_JUMPAPEX_R = "JumpApexRight";
    const string PLAYER_HANGINGLEDGE_R = "LedgeHangRight";
    const string PLAYER_CLIMBLEDGE_R = "ClimbLedgeRight";

    //Left side
    const string PLAYER_IDLE_L = "IdleLeft";
    const string PLAYER_WALK_L = "WalkLeft";
    const string PLAYER_WALLSLIDE_L = "WallSlideLeft";
    const string PLAYER_RUNNING_L = "RunLeft";
    const string PLAYER_GOPRONE_L = "GoProneLeft";
    const string PLAYER_CRAWLING_L = "CrawlingLeft";
    const string PLAYER_STANDUP_L = "StandUpLeft";
    const string PLAYER_SLIDE_L = "SlideLeft";
    const string PLAYER_FALL_L = "FallLeft";
    const string PLAYER_JUMP_L = "JumpUpLeft";
    const string PLAYER_JUMPSLOW_L = "JumpUpSlowLeft";
    const string PLAYER_JUMPAPEX_L = "JumpApexLeft";
    const string PLAYER_HANGINGLEDGE_L = "LedgeHangLeft";
    const string PLAYER_CLIMBLEDGE_L = "ClimbLedgeLeft";


    private void Start()
    {

    }

    internal void AnimationManager() // conditions to play animations
    {
        if (playerController.playerMovement.isClimbingLedge)
        {
            if (playerController.playerMovement.isFacingRight
             && playerController.currentState != PLAYER_CLIMBLEDGE_L) //so it wouldn't flip while climbing and pressing a/d
            {
                playerController.ChangeAnimationState(PLAYER_CLIMBLEDGE_R);
            }
            else if (playerController.currentState != PLAYER_CLIMBLEDGE_R)
            {
                playerController.ChangeAnimationState(PLAYER_CLIMBLEDGE_L);
            }
        }

        else if (playerController.playerSurroundings.isGrounded)
        {

            if (playerController.playerMovement.isCrawlingNew || playerController.playerMovement.isProneIdle)
            {
                if (playerController.crawlTimer > 0)
                {
                    playerController.crawlTimer -= Time.deltaTime;
                    playerController.standUpTimer = playerController.standUpTimerSet;

                    if (playerController.playerMovement.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_GOPRONE_R);

                    }

                    else if (!playerController.playerMovement.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_GOPRONE_L);
                    }
                }

                else if (playerController.playerMovement.isProneIdle)
                {
                    if (playerController.playerMovement.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_CRAWLING_R);

                    }

                    else
                    {
                        playerController.ChangeAnimationState(PLAYER_CRAWLING_L);
                    }
                }

                else if (playerController.playerMovement.isCrawlingNew/*playerController.crawlTimer <= 0
                     && !playerController.playerMovement.isSlidingNew*/)
                {
                    if (playerController.playerMovement.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_CRAWLING_R);

                    }
                    else
                    {
                        playerController.ChangeAnimationState(PLAYER_CRAWLING_L);
                    }
                }
            }

          

            else if (!playerController.playerMovement.isCrawlingNew
                   && playerController.playerInput.isJumpPressed
                   && !playerController.playerSurroundings.isGrounded)
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_JUMP_R);
                }
            }



            else if (playerController.playerMovement.isSprintingNew
                 
                 && !playerController.playerSurroundings.isTouchingWall
                 && !playerController.playerSurroundings.isTouchingLedge)
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_RUNNING_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_RUNNING_L);
                }
            }
            else if (playerController.playerMovement.isSlidingNew)
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_SLIDE_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_SLIDE_L);
                }
            }


            else if ((playerController.currentState == PLAYER_CRAWLING_R
                  && !playerController.playerMovement.isCrawlingNew
                  && playerController.standUpTimer > 0)
                      ||
                     (playerController.currentState == PLAYER_CRAWLING_R
                  && playerController.playerMovement.isWalkingNew
                  && !playerController.playerMovement.isCrawlingNew
                  && playerController.standUpTimer > 0))

            {
                playerController.crawlTimer = playerController.crawlTimerSet;

                if (playerController.playerMovement.isFacingRight
                 && playerController.currentState == PLAYER_CRAWLING_R)

                {
                    playerController.ChangeAnimationState(PLAYER_STANDUP_R);
                }

            }
            else if ((playerController.currentState == PLAYER_CRAWLING_L
                  && !playerController.playerMovement.isCrawlingNew
                  && playerController.standUpTimer > 0)
                      ||
                     (playerController.currentState == PLAYER_CRAWLING_L
                  && playerController.playerMovement.isWalkingNew
                  && !playerController.playerMovement.isCrawlingNew
                  && playerController.standUpTimer > 0))
            {
                playerController.crawlTimer = playerController.crawlTimerSet;

                if (!playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_STANDUP_L);
                }

            }
            else if (playerController.currentState == PLAYER_STANDUP_R
                  && playerController.standUpTimer > 0)
            {
                playerController.standUpTimer -= Time.deltaTime;

                if (playerController.standUpTimer <= 0)
                {
                    if (playerController.playerMovement.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_IDLE);

                    }
                }
            }

            else if (playerController.currentState == PLAYER_STANDUP_L
                  && playerController.standUpTimer > 0 /*&& isWalkingNew*/)
            {
                playerController.standUpTimer -= Time.deltaTime;

                if (playerController.standUpTimer <= 0)
                {
                    playerController.ChangeAnimationState(PLAYER_IDLE_L);
                    playerController.crawlTimer = playerController.crawlTimerSet;
                }
            }

            else if (playerController.playerMovement.isWalkingNew
                 && !playerController.playerMovement.isCrawlingNew
                 && !playerController.playerSurroundings.isTouchingWall
                 && !playerController.playerSurroundings.isTouchingLedge)
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_WALK_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_WALK_L);
                }
            }
            else
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_IDLE);
                }

                else
                {
                    playerController.ChangeAnimationState(PLAYER_IDLE_L);
                }
            }

        }

        else if (playerController.playerMovement.isWallSliding)
        {
            if (playerController.playerMovement.isFacingRight)
            {

                playerController.ChangeAnimationState(PLAYER_WALLSLIDE_R);
            }
            else
            {
                playerController.ChangeAnimationState(PLAYER_WALLSLIDE_L);
            }
        }

        else if (!playerController.playerSurroundings.isGrounded)
        {

            if (playerController.playerMovement.isHangingLedge
            && !playerController.playerMovement.isClimbingLedge)
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_HANGINGLEDGE_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_HANGINGLEDGE_L);
                }
            }

            else if (playerController.rb.velocity.y > 0
                  && playerController.playerMovement.isFacingRight
                   && !playerController.playerSurroundings.isGrounded)
            {
                playerController.ChangeAnimationState(PLAYER_JUMP_R);
            }

            else if (playerController.rb.velocity.y > 0
                 && !playerController.playerMovement.isFacingRight
                  && !playerController.playerSurroundings.isGrounded)
            {
                playerController.ChangeAnimationState(PLAYER_JUMP_L);
            }

            else if (playerController.rb.velocity.y < -2
                 && !playerController.playerMovement.isWallSliding
                 && !playerController.playerSurroundings.canJump)
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_FALL_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_FALL_L);
                }
            }

            else if (playerController.rb.velocity.y > 0
                  && playerController.rb.velocity.y < 2
                  && playerController.playerMovement.isFacingRight /*&& playerController.currentState != PLAYER_JUMP_R*/)
            {
                playerController.ChangeAnimationState(PLAYER_JUMPSLOW_R);
            }

            else if (playerController.rb.velocity.y > 0
                  && playerController.rb.velocity.y < 2
                  && !playerController.playerMovement.isFacingRight)
            // && playerController.currentState != PLAYER_JUMP_L)
            {
                playerController.ChangeAnimationState(PLAYER_JUMPSLOW_L);
            }


            else if (playerController.rb.velocity.y < 1
                  && playerController.rb.velocity.y > -1
                  && !playerController.playerMovement.isHangingLedge
                  && !playerController.playerSurroundings.isGrounded)
            {
                if (playerController.playerMovement.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_JUMPAPEX_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_JUMPAPEX_L);
                }
            }

        }



        else
        {
            playerController.ChangeAnimationState(PLAYER_IDLE);
            //playerController.crawlTimer = playerController.crawlTimerSet;
        }


    }




}
