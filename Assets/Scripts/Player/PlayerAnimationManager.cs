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
    const string PLAYER_CRAWLING_R = "CrawlRight";
    const string PLAYER_STANDUP_R = "StandUpRight";
    const string PLAYER_SLIDE_R = "SlideRight";
    const string PLAYER_FALL_R = "FallRight";
    const string PLAYER_JUMP_R = "JumpUpRight";
    const string PLAYER_JUMPSLOW_R = "JumpUpSlowRight";
    const string PLAYER_JUMPAPEX_R = "JumpApexRight";
    const string PLAYER_HANGINGLEDGE_R = "LedgeHangRight";
    const string PLAYER_CLIMBLEDGE_R = "ClimbLedgeRight";
    const string PLAYER_WALLJUMPREADY_R = "WallJumpReadyRight";

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
    const string PLAYER_WALLJUMPREADY_L = "WallJumpReadyLeft";




    const string PLAYER_STUCK_IN_GROUND = "StuckInGround";


    private void Start()
    {

    }


    internal void AnimationManagerStateTest()
    {
        switch (playerController.playerState.currentState)
        {
            //-----------------------------------------------------------------I D L E-------------------------------------------
            case PlayerState.CharacterMovement.Idle:

                if (playerController.playerState.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_IDLE);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_IDLE_L);
                }

                break;

            //-----------------------------------------------------------------W A L K I N G-------------------------------------------
            case PlayerState.CharacterMovement.Walking:

                if (playerController.playerState.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_WALK_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_WALK_L);
                }

                break;

            //-----------------------------------------------------------------S P R I N T I N G-------------------------------------------
            case PlayerState.CharacterMovement.Sprinting:

                if (playerController.playerState.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_RUNNING_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_RUNNING_L);
                }

                break;

            //-----------------------------------------------------------------P R O N E-------------------------------------------
            case PlayerState.CharacterMovement.Prone:

                if (playerController.playerTimers.crawlTimer < playerController.playerTimers.crawlTimerSet)
                {
                    playerController.playerTimers.crawlTimer += Time.deltaTime;
                    playerController.playerTimers.standUpTimer = playerController.playerTimers.standUpTimerSet;

                    if (playerController.playerState.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_GOPRONE_R);

                    }

                    else if (!playerController.playerState.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_GOPRONE_L);
                    }
                }

                else
                {
                    if (playerController.playerState.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_CRAWLING_R);

                    }
                    else
                    {
                        playerController.ChangeAnimationState(PLAYER_CRAWLING_L);
                    }
                }

                break;

            //-----------------------------------------------------------------C R A W L I N G-------------------------------------------
            case PlayerState.CharacterMovement.Crawling:

                if (playerController.playerTimers.crawlTimer < playerController.playerTimers.crawlTimerSet)
                {
                    playerController.playerTimers.crawlTimer += Time.deltaTime;
                    playerController.playerTimers.standUpTimer = playerController.playerTimers.standUpTimerSet;

                    if (playerController.playerState.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_GOPRONE_R);

                    }

                    else if (!playerController.playerState.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_GOPRONE_L);
                    }
                }

                else
                {
                    if (playerController.playerState.isFacingRight)
                    {
                        playerController.ChangeAnimationState(PLAYER_CRAWLING_R);

                    }
                    else
                    {
                        playerController.ChangeAnimationState(PLAYER_CRAWLING_L);
                    }
                }

                break;

            //-----------------------------------------------------------------S L I D I N G-------------------------------------------
            case PlayerState.CharacterMovement.Sliding:

                if (playerController.playerState.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_SLIDE_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_SLIDE_L);
                }

                break;

            //-----------------------------------------------------------------J U M P I N G-------------------------------------------
            case PlayerState.CharacterMovement.Jumping:

                if (playerController.playerState.isFacingRight)
                {
                    if (playerController.rb.velocity.y < 1)
                    {
                        playerController.ChangeAnimationState(PLAYER_JUMPAPEX_R);
                    }
                    else
                    {
                        playerController.ChangeAnimationState(PLAYER_JUMP_R);
                    }
                }
                else
                {
                    if (playerController.rb.velocity.y < 1)
                    {
                        playerController.ChangeAnimationState(PLAYER_JUMPAPEX_L);
                    }
                    else
                    {
                        playerController.ChangeAnimationState(PLAYER_JUMP_L);
                    }
                }

                break;

            //-----------------------------------------------------------------F A L L I N G-------------------------------------------
            case PlayerState.CharacterMovement.Falling:

                if (playerController.playerState.isFacingRight)
                {
                    playerController.ChangeAnimationState(PLAYER_FALL_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_FALL_L);
                }

                break;

            //-----------------------------------------------------------------W A L L S L I D I N G-------------------------------------------
            case PlayerState.CharacterMovement.Wallsliding:

                if (playerController.playerState.isFacingRight)
                {

                    playerController.ChangeAnimationState(PLAYER_WALLSLIDE_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_WALLSLIDE_L);
                }

                break;

            //-----------------------------------------------------------------H A N G I N G-------------------------------------------
            case PlayerState.CharacterMovement.HangingLedge:

                if (playerController.playerState.isFacingRight)
                {

                    playerController.ChangeAnimationState(PLAYER_HANGINGLEDGE_R);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_HANGINGLEDGE_L);
                }

                break;

            //-----------------------------------------------------------------W A L L J U M P-------------------------------------------
            case PlayerState.CharacterMovement.WallJump:

                if (playerController.playerState.isFacingRight)
                {

                    playerController.ChangeAnimationState(PLAYER_WALLJUMPREADY_L);
                }
                else
                {
                    playerController.ChangeAnimationState(PLAYER_WALLJUMPREADY_R);
                }

                break;

            case PlayerState.CharacterMovement.Climbing:

                if (playerController.playerState.isFacingRight
                && playerController.currentState != PLAYER_CLIMBLEDGE_L) //so it wouldn't flip while climbing and pressing a/d
               {
                    playerController.ChangeAnimationState(PLAYER_CLIMBLEDGE_R);
                }
               else if (playerController.currentState != PLAYER_CLIMBLEDGE_R)
                {
                    playerController.ChangeAnimationState(PLAYER_CLIMBLEDGE_L);
                }
                break;
        }
    }

    internal void AnimationManager() // conditions to play animations
    {
        if (playerController.playerState.isStuckInGround)
        {
            playerController.ChangeAnimationState(PLAYER_STUCK_IN_GROUND);
        }

        else
        {
            // if (playerController.playerMovement.isClimbingLedge)
            // {
            //     if (playerController.playerState.isFacingRight
            //      && playerController.currentState != PLAYER_CLIMBLEDGE_L) //so it wouldn't flip while climbing and pressing a/d
            //     {
            //         playerController.ChangeAnimationState(PLAYER_CLIMBLEDGE_R);
            //     }
            //     else if (playerController.currentState != PLAYER_CLIMBLEDGE_R)
            //     {
            //         playerController.ChangeAnimationState(PLAYER_CLIMBLEDGE_L);
            //     }
            // }
            //
            // else if (playerController.playerSurroundings.isGrounded)
            // {
            //
            //    // if (playerController.playerMovement.isCrawling || playerController.playerMovement.isProneIdle)
            //    // {
            //    //     if (playerController.playerTimers.crawlTimer > 0)
            //    //     {
            //    //         playerController.playerTimers.crawlTimer -= Time.deltaTime;
            //    //         playerController.playerTimers.standUpTimer = playerController.playerTimers.standUpTimerSet;
            //    //
            //    //         if (playerController.playerState.isFacingRight)
            //    //         {
            //    //             playerController.ChangeAnimationState(PLAYER_GOPRONE_R);
            //    //
            //    //         }
            //    //
            //    //         else if (!playerController.playerState.isFacingRight)
            //    //         {
            //    //             playerController.ChangeAnimationState(PLAYER_GOPRONE_L);
            //    //         }
            //    //     }
            //    //
            //    //    // else if (playerController.playerMovement.isProneIdle)
            //    //    // {
            //    //    //     if (playerController.playerState.isFacingRight)
            //    //    //     {
            //    //    //         playerController.ChangeAnimationState(PLAYER_CRAWLING_R);
            //    //    //
            //    //    //     }
            //    //    //
            //    //    //     else
            //    //    //     {
            //    //    //         playerController.ChangeAnimationState(PLAYER_CRAWLING_L);
            //    //    //     }
            //    //    // }
            //    //
            //    //    // else if (playerController.playerMovement.isCrawling/*playerController.crawlTimer <= 0
            //    //    //  && !playerController.playerMovement.isSlidingNew*/)
            //    //    // {
            //    //    //     if (playerController.playerState.isFacingRight)
            //    //    //     {
            //    //    //         playerController.ChangeAnimationState(PLAYER_CRAWLING_R);
            //    //    //
            //    //    //     }
            //    //    //     else
            //    //    //     {
            //    //    //         playerController.ChangeAnimationState(PLAYER_CRAWLING_L);
            //    //    //     }
            //    //    // }
            //    // }
            //    //
            //    //
            //    //
            //    // else if (!playerController.playerMovement.isCrawling
            //    //        && playerController.playerInput.isJumpPressed
            //    //        && !playerController.playerSurroundings.isGrounded)
            //    // {
            //    //     if (playerController.playerState.isFacingRight)
            //    //     {
            //    //         playerController.ChangeAnimationState(PLAYER_JUMP_R);
            //    //     }
            //    // }
            //
            //
            //
            //    // else if (playerController.playerMovement.isSprinting
            //    //
            //    //      && !playerController.playerSurroundings.isTouchingWall
            //    //      && !playerController.playerSurroundings.isTouchingLedge)
            //    // {
            //    //     if (playerController.playerState.isFacingRight)
            //    //     {
            //    //         playerController.ChangeAnimationState(PLAYER_RUNNING_R);
            //    //     }
            //    //     else
            //    //     {
            //    //         playerController.ChangeAnimationState(PLAYER_RUNNING_L);
            //    //     }
            //    // }
            //    // else if (playerController.playerMovement.isSliding)
            //    // {
            //    //     if (playerController.playerState.isFacingRight)
            //    //     {
            //    //         playerController.ChangeAnimationState(PLAYER_SLIDE_R);
            //    //     }
            //    //     else
            //    //     {
            //    //         playerController.ChangeAnimationState(PLAYER_SLIDE_L);
            //    //     }
            //    // }
            //
            //
            //     // else if ((playerController.currentState == PLAYER_CRAWLING_R
            //     //       && !playerController.playerMovement.isCrawling
            //     //       && playerController.playerTimers.standUpTimer > 0)
            //     //           ||
            //     //          (playerController.currentState == PLAYER_CRAWLING_R
            //     //       && playerController.playerMovement.isWalking
            //     //       && !playerController.playerMovement.isCrawling
            //     //       && playerController.playerTimers.standUpTimer > 0))
            //     //
            //     // {
            //     //     playerController.playerTimers.crawlTimer = 0;
            //     //
            //     //     if (playerController.playerState.isFacingRight
            //     //      && playerController.currentState == PLAYER_CRAWLING_R)
            //     //
            //     //     {
            //     //         playerController.ChangeAnimationState(PLAYER_STANDUP_R);
            //     //     }
            //
            //     //}
            //     // else if ((playerController.currentState == PLAYER_CRAWLING_L
            //     //       && !playerController.playerMovement.isCrawling
            //     //       && playerController.playerTimers.standUpTimer > 0)
            //     //           ||
            //     //          (playerController.currentState == PLAYER_CRAWLING_L
            //     //       && playerController.playerMovement.isWalking
            //     //       && !playerController.playerMovement.isCrawling
            //     //       && playerController.playerTimers.standUpTimer > 0))
            //     // {
            //     //     playerController.playerTimers.crawlTimer = 0;
            //     //
            //     //     if (!playerController.playerState.isFacingRight)
            //     //     {
            //     //         playerController.ChangeAnimationState(PLAYER_STANDUP_L);
            //     //     }
            //     //
            //     // }
            //   //  else if (playerController.currentState == PLAYER_STANDUP_R
            //   //        && playerController.playerTimers.standUpTimer > 0)
            //   //  {
            //   //      playerController.playerTimers.standUpTimer -= Time.deltaTime;
            //   //
            //   //      if (playerController.playerTimers.standUpTimer <= 0)
            //   //      {
            //   //          if (playerController.playerState.isFacingRight)
            //   //          {
            //   //              playerController.ChangeAnimationState(PLAYER_IDLE);
            //   //
            //   //          }
            //   //      }
            //   //  }
            //   //
            //   //  else if (playerController.currentState == PLAYER_STANDUP_L
            //   //        && playerController.playerTimers.standUpTimer > 0 /*&& isWalking*/)
            //   //  {
            //   //      playerController.playerTimers.standUpTimer -= Time.deltaTime;
            //   //
            //   //      if (playerController.playerTimers.standUpTimer <= 0)
            //   //      {
            //   //          playerController.ChangeAnimationState(PLAYER_IDLE_L);
            //   //          playerController.playerTimers.crawlTimer = 0;
            //   //      }
            //   //  }
            //
            //     // else if (playerController.playerMovement.isWalking
            //     //      && !playerController.playerMovement.isCrawling
            //     //      && !playerController.playerSurroundings.isTouchingWall
            //     //      && !playerController.playerSurroundings.isTouchingLedge)
            //     // {
            //     //     if (playerController.playerState.isFacingRight)
            //     //     {
            //     //         playerController.ChangeAnimationState(PLAYER_WALK_R);
            //     //     }
            //     //     else
            //     //     {
            //     //         playerController.ChangeAnimationState(PLAYER_WALK_L);
            //     //     }
            //     // }
            //   //  else
            //   //  {
            //   //      if (playerController.playerState.isFacingRight)
            //   //      {
            //   //          playerController.ChangeAnimationState(PLAYER_IDLE);
            //   //      }
            //   //
            //   //      else
            //   //      {
            //   //          playerController.ChangeAnimationState(PLAYER_IDLE_L);
            //   //      }
            //   //  }
            //
            // }

            // else if (playerController.playerMovement.wallJumpReady)
            // {
            //     if (playerController.playerState.isFacingRight)
            //     {
            //         playerController.ChangeAnimationState(PLAYER_WALLJUMPREADY_L);
            //     }
            //
            //     else
            //     {
            //         playerController.ChangeAnimationState(PLAYER_WALLJUMPREADY_R);
            //     }
            // }

            // else if (playerController.playerMovement.isWallSliding)
            // {
            //     if (playerController.playerState.isFacingRight)
            //     {
            //
            //         playerController.ChangeAnimationState(PLAYER_WALLSLIDE_R);
            //     }
            //     else
            //     {
            //         playerController.ChangeAnimationState(PLAYER_WALLSLIDE_L);
            //     }
            // }
            //
            // else if (!playerController.playerSurroundings.isGrounded)
            // {
            //
            //     if (playerController.playerMovement.isHangingLedge
            //     && !playerController.playerMovement.isClimbingLedge)
            //     {
            //         if (playerController.playerState.isFacingRight)
            //         {
            //             playerController.ChangeAnimationState(PLAYER_HANGINGLEDGE_R);
            //         }
            //         else
            //         {
            //             playerController.ChangeAnimationState(PLAYER_HANGINGLEDGE_L);
            //         }
            //     }
            //
            //     else if (playerController.rb.velocity.y > 0
            //           && playerController.playerState.isFacingRight
            //            && !playerController.playerSurroundings.isGrounded)
            //     {
            //         playerController.ChangeAnimationState(PLAYER_JUMP_R);
            //     }
            //
            //     else if (playerController.rb.velocity.y > 0
            //          && !playerController.playerState.isFacingRight
            //           && !playerController.playerSurroundings.isGrounded)
            //     {
            //         playerController.ChangeAnimationState(PLAYER_JUMP_L);
            //     }
            //
            //    // else if (playerController.rb.velocity.y < -2
            //    //      && !playerController.playerMovement.isWallSliding
            //    //      && !playerController.playerState.canJump)
            //    // {
            //    //     if (playerController.playerState.isFacingRight)
            //    //     {
            //    //         playerController.ChangeAnimationState(PLAYER_FALL_R);
            //    //     }
            //    //     else
            //    //     {
            //    //         playerController.ChangeAnimationState(PLAYER_FALL_L);
            //    //     }
            //    // }
            //
            //     else if (playerController.rb.velocity.y > 0
            //           && playerController.rb.velocity.y < 2
            //           && playerController.playerState.isFacingRight /*&& playerController.currentState != PLAYER_JUMP_R*/)
            //     {
            //         playerController.ChangeAnimationState(PLAYER_JUMPSLOW_R);
            //     }
            //
            //     else if (playerController.rb.velocity.y > 0
            //           && playerController.rb.velocity.y < 2
            //           && !playerController.playerState.isFacingRight)
            //     // && playerController.currentState != PLAYER_JUMP_L)
            //     {
            //         playerController.ChangeAnimationState(PLAYER_JUMPSLOW_L);
            //     }
            //
            //
            //     else if (playerController.rb.velocity.y < 1
            //           && playerController.rb.velocity.y > -1
            //           && !playerController.playerMovement.isHangingLedge
            //           && !playerController.playerSurroundings.isGrounded)
            //     {
            //         if (playerController.playerState.isFacingRight)
            //         {
            //             playerController.ChangeAnimationState(PLAYER_JUMPAPEX_R);
            //         }
            //         else
            //         {
            //             playerController.ChangeAnimationState(PLAYER_JUMPAPEX_L);
            //         }
            //     }
            //
            // }



            // else
            // {
            //     playerController.ChangeAnimationState(PLAYER_IDLE);
            //     //playerController.crawlTimer = playerController.crawlTimerSet;
            // }
        }


    }




}
