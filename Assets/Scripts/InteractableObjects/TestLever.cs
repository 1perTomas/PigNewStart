using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLever : MonoBehaviour
{
    internal GameObject target;

    [SerializeField]
    internal GameObject player;

    internal PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        playerController = player.GetComponent<PlayerController>();
        if (playerController.playerState.currentState == PlayerState.CharacterMovement.Interacting && playerController.playerInput.isLeftTapped)
        {
            Debug.Log(target.name);
            GoUp();
        }
        // Debug.Log(target.name);
    }

    internal void GoUp()
    {
        target.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + 1);
    }
}
