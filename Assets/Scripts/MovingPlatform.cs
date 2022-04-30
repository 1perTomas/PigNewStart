using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    internal Vector3 velocity;

    [SerializeField]
    PlayerController playerController;


    internal bool onPlatform = false;
    private Rigidbody2D platform;
    float width;

    private void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    

      if (playerController.playerMovement.isClimbingLedge && onPlatform)
      {
          if (playerController.transform.localPosition.x > 0)
          {
              playerController.transform.position = new Vector2(gameObject.transform.position.x + (width / 2 - 0.18f), gameObject.transform.position.y + 1f);
          }
     
          else
          {
              playerController.transform.position = new Vector2(gameObject.transform.position.x - (width / 2 - 0.18f), gameObject.transform.position.y + 1f);
          }
      }
        // Vector3 tmp = transform.position;
        // tmp.x = Mathf.Round(tmp.x);
        // tmp.y = Mathf.Round(tmp.y);
        //
       // NearestPixel();
        transform.position += (velocity * Time.deltaTime);

       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (playerController.playerMovement.isClimbingLedge)
        // {
        //     playerController.rb.position = new Vector2(transform.position.x - 0.35f, transform.position.y + 0.28f);
        // }



        if (collision.gameObject.tag == "Player")
        {
            print("He has landed");
           onPlatform = true;
            collision.collider.transform.SetParent(transform);
        }



        // else if (playerController.playerMovement.isHangingLedge)
        // {
        //     collision.collider.transform.SetParent(transform);
        //     playerController.playerMovement.velX = platform.velocity.x;
        //     playerController.playerMovement.velY = platform.velocity.y;
        // }




    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onPlatform = false;
            collision.collider.transform.SetParent(null);
        }

    }

    // private void OnCollisionStay2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //        
    //         if (playerController.playerMovement.isHangingLedge && !playerController.playerMovement.isClimbingLedge)
    //
    //         {
    //             collision.collider.transform.SetParent(transform);
    //            // playerController.playerMovement.velX = platform.velocity.x;
    //             //playerController.playerMovement.velY = platform.velocity.y;
    //         }
    //     }
    // }



    private void NearestPixel()
    {
       
            float pixelCoord = Mathf.Round(transform.localPosition.x / 0.03125f);
            float pixelPos = (pixelCoord * 0.03125f);
           transform.localPosition = new Vector3(pixelPos, transform.localPosition.y, transform.localPosition.z);
      
    }





}
