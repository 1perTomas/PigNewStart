using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectObject : MonoBehaviour
{
    [SerializeField]
    internal PlayerController playerController;

    internal bool isTouchingObject;
    internal RaycastHit2D objectItself;

    internal string objectType;
    internal bool canInteract;



    internal void FindObject()
    {
        isTouchingObject = playerController.playerSurroundings.isTouchingInteractableObject;
        objectItself = playerController.playerSurroundings.isTouchingInteractableObject;
    }

    internal void CheckForObjects()
    {

        if (!playerController.playerState.isInteracting) //finds interactables if not interacting, prevents affecting walls
        {
            FindObject();
        }

        if (isTouchingObject && !playerController.playerState.isInteracting) //prevents from taking tags while interacting
        {

            if (objectItself.collider.CompareTag("MovableObject"))
            {
                IdentifyObjectType();
            }

            else if (objectItself.collider.CompareTag("CarriableObject"))
            {
                IdentifyObjectType();
            }
        }

        else if (!playerController.playerState.isInteracting) //wipes info if not interacting or touching
        {
            isTouchingObject = false;
            objectType = null;
            canInteract = false;
        }
    }

    internal void IdentifyObjectType()
    {
        string objectTag = objectItself.collider.tag.ToString(); // takes the objects tag and turns to string
        canInteract = true;
        if (objectType == null) // removes the "object" string from the tag
        {
            objectType = objectTag.Remove(objectTag.Length - 6);
        }
    }
}
