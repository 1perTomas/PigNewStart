using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{


    internal Vector2 StandingBox;
    internal Vector2 CrawlingBox;
    internal Vector2 InteractingBox;


    // Start is called before the first frame update
    void Start()
    {
        StandingBox = new Vector2(0.34f, 0.83f);
        CrawlingBox = new Vector2(0.34f, 0.415f);



    }

    // Update is called once per frame
    void Update()
    {

    }


    private void BoxColliderFull()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0.34f, 0.83f);
        GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.069f);
    }

    private void BoxColliderProne()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0.34f, 0.415f); // full y size / 2
        GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.2765f); // 0 - (prone y size /2)  + (full y offset /2) 
    }






}
