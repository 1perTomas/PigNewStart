using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLever : MonoBehaviour
{
    internal GameObject target;



    // Start is called before the first frame update
    void Start()
    {
        target = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(target.name);
    }

    internal void GoUp()
    {
        target.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + 1);
    }
}
