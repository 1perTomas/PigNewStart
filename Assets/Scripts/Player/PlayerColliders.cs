using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliders : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    internal Vector2 StandingBox;
    internal Vector2 CrawlingBox;
    internal Vector2 InteractingBox;

    void Start()
    {
        StandingBox = new Vector2(0.34f, 0.83f);
        CrawlingBox = new Vector2(0.34f, 0.415f);
    }

    private void BoxColliderFull()
    {
        GetComponent<BoxCollider2D>().size = StandingBox;
        GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.069f);
    }

    private void BoxColliderProne()
    {
        GetComponent<BoxCollider2D>().size = CrawlingBox; // full y size / 2
        GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.2765f); // 0 - (prone y size /2)  + (full y offset /2) 
    }

    internal void AdjustColliderBoxMovable(int direction)
    {
        playerController.GetComponent<BoxCollider2D>().offset = new Vector2(direction * ((StandingBox.x + playerController.playerDetectObject.touchingObject.GetComponent<SpriteRenderer>().bounds.size.x) / 2 - (StandingBox.x / 2)), -0.069f);
        playerController.GetComponent<BoxCollider2D>().size = new Vector2(playerController.playerDetectObject.touchingObject.GetComponent<SpriteRenderer>().bounds.size.x + StandingBox.x, StandingBox.y);
    }

    internal void AdjustColliderBoxCarriable(int direction)
    {
        // needs another raycast to see if the carriable hits any obstacles, if it does - adjust collider to it's size
        playerController.GetComponent<BoxCollider2D>().offset = new Vector2(direction * 0.015f, (playerController.playerDetectObject.touchingObject.GetComponent<SpriteRenderer>().bounds.size.y) / 2 - 0.069f);
        playerController.GetComponent<BoxCollider2D>().size = new Vector2(StandingBox.x, playerController.playerDetectObject.touchingObject.GetComponent<SpriteRenderer>().bounds.size.y + StandingBox.y);
    }



    internal void ColliderAdjust()
    {
        if (playerController.playerState.isInteracting)
        {
            if (playerController.playerDetectObject.objectType == "Movable")
            {
                if (playerController.playerState.isFacingRight)
                {
                    AdjustColliderBoxMovable(1);
                }
                else
                {
                    AdjustColliderBoxMovable(-1);
                }
            }

            else if (playerController.playerDetectObject.objectType == "Carriable")
            {
                if (playerController.playerInteraction.isCarrying)
                {
                    if (playerController.playerState.isFacingRight)
                    {
                        //playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = new Vector2(0, 0.8f);
                        AdjustColliderBoxCarriable(1);
                    }

                    else
                    {
                        //playerController.playerDetectObject.touchingObject.GetComponent<Rigidbody2D>().transform.localPosition = new Vector2(0, 0.8f);
                        AdjustColliderBoxCarriable(-1);
                    }
                }

                else
                {
                    if (playerController.playerState.isFacingRight)
                    {
                        AdjustColliderBoxMovable(1);
                    }
                    else
                    {
                        AdjustColliderBoxMovable(-1);
                    }
                }

            }
        }

        else
        {
            if (playerController.playerMovement.isStanding)
            {
                BoxColliderFull();
            }
            else if (!playerController.playerMovement.isStanding)
            {
                BoxColliderProne();
            }
        }
    }



}
