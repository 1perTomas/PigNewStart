using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField]
    internal PlayerController playerController;

    public float bounceBack;
    public float bounceUp;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DangerousObject") && playerController.playerState.controlMode == PlayerState.ControlMode.FreeMovement)
        {
            playerController.speedList.currentSpeed = bounceBack;

            playerController.playerState.currentState = PlayerState.CharacterMovement.KnockBack;

            playerController.playerState.controlMode = PlayerState.ControlMode.Damaged;

            MinusHealth();

            if ((collision.gameObject.transform.position.x >= playerController.rb.transform.position.x && bounceBack > 1)
                || (collision.gameObject.transform.position.x < playerController.rb.transform.position.x && bounceBack < 1))
            {
                bounceBack *= -1;
                Knockback();
            }

            else
            {
                Knockback();
            }

        }



    }

    internal void MinusHealth()
    {

        if (playerController.playerState.health > 0)
        {
            playerController.playerState.health -= 1;
            Debug.Log($"you took damage, your health is {playerController.playerState.health}");
        }
    }



    internal void Knockback()
    {
        playerController.speedList.currentSpeed = bounceBack;

        playerController.rb.velocity = new Vector2(bounceBack, bounceUp);

        playerController.rb.position = new Vector2(playerController.rb.position.x - 0.1f, playerController.rb.position.y + 0.1f);
    }


}
