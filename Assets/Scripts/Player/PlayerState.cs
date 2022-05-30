using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    internal Vector2 StandingBox;
    internal Vector2 CrawlingBox;
    internal Vector2 InteractingBox;

    internal bool isFacingRight = true;
    internal bool isMoving;
    internal bool isInteracting = false;


    // Start is called before the first frame update
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

    internal void ColliderAdjust()
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
