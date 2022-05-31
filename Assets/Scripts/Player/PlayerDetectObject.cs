using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectObject : MonoBehaviour
{
    [SerializeField]
    internal PlayerController playerController;

    RaycastHit2D canInteract;

    internal void FindObject()
    {
        canInteract = playerController.playerSurroundings.isTouchingMovableObject;
    }
}
