using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newFollow : MonoBehaviour
{
    private float horizontalOffset = 0.9f;
    private float verticalOffset = 0.8f;

    private float pixelPos;

    public PixelPerfectCam pixelCam;


    public PlayerController playerController;

    public Transform followTransform;
    public float ratio;
    public float layerDepth;

    private Vector3 startPosition;
    public float startZ;
    public float smoothSpeed;



    Vector2 travel => (Vector3)followTransform.transform.position - startPosition;
    Vector2 parallaxFactor;


    public float cameraLocX;
    private bool isFacingRight;
    private bool isClimbing;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, layerDepth); // cuts off z axis
        //startZ = transform.position.z - layerDepth;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<PixelPerfectCam>();
        GetComponent<PlayerController>();

        isFacingRight = playerController.playerState.isFacingRight;
        isClimbing = pixelCam.isClimbingLedge;
        cameraLocX = pixelCam.transform.position.x;
    }

    private void LateUpdate()
    {
        NearestPixel();
        CameraAdjust();
        CameraAdjustClimbing();
    }

    private void CameraAdjustClimbing()
    {
        if (isClimbing && pixelCam.climbDif != null)
        {
            Vector3 desiredPosition = new Vector3((transform.position.x + pixelCam.climbDif.x), startPosition.y, layerDepth);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * ratio * Time.deltaTime);
        }
    }

    private void CameraAdjust()
    {
        if (pixelCam.transform.position.x > cameraLocX)
        {
            transform.position = new Vector3(pixelPos + horizontalOffset, startPosition.y/* + travel.y * ratio) + 0.8f*/, layerDepth);
        }
        else if (pixelCam.transform.position.x < cameraLocX)
        {

            transform.position = new Vector3(pixelPos + horizontalOffset, startPosition.y/* + travel.y * ratio) + 0.8f*/, layerDepth);
        }
    }

    private void NearestPixel()
    {
        float pixelCoord = Mathf.Round(((startPosition.x + travel.x * ratio)) / (0.03125f / 64)); // added /4 for smoother movement
        pixelPos = (pixelCoord * 0.03125f / 64);
    }
}
