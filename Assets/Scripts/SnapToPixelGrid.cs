using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPixelGrid : MonoBehaviour
{
    [SerializeField]
    private int pixelsPerUnit = 32;

    private Transform parent;

    private GameObject box;
    private Rigidbody2D rb;

    private void Start()
    {
        box = GetComponent<GameObject>();
       rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Snap the object to the pixel grid determined by the given pixelsPerUnit.
    /// Using the parent's world position, this moves to the nearest pixel grid location by 
    /// offseting this GameObject by the difference between the parent position and pixel grid.
    /// </summary>
    private void LateUpdate()
    { //
      // if (rb.simulated)
      // {
            NearestPixel();
        //}
    }

    private void NearestPixel()
    {

        float pixelCoordx = Mathf.Round(transform.localPosition.x / 0.03125f);
        float pixelPosx = (pixelCoordx * 0.03125f);
        float pixelCoordy = Mathf.Round(transform.localPosition.x / 0.03125f);
        float pixelPosy = (pixelCoordx * 0.03125f);

        transform.localPosition = new Vector3(pixelPosx, transform.localPosition.y, transform.localPosition.z);

    }
}
