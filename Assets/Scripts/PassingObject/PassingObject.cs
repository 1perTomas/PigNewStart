using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PassingObject : MonoBehaviour
{

    private bool entered;

    private Animator anim;
    private string wheatPass;
    private string objName;



    private void Start()
    {
        anim = GetComponent<Animator>();
        objName = gameObject.name;
        wheatPass = objName.Substring(0, 10) + "Pass";
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            entered = true;
            playAnim();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entered = false;
    }

    private void playAnim()
    {
        if (entered)
        {
            anim.Play(wheatPass);
        }
    }


}
