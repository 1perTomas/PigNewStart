using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PixelPerfectCam : MonoBehaviour
{
    [SerializeField]
    MovingPlatform platform;

    [SerializeField]
   PlayerController playerController;
    private float horizontalOffset = 0.9f;
    private float verticalOffset = 0.8f;
    public bool isHangingLedge;
    public bool isClimbingLedge;
   // public bool isInZone;


    public float smoothSpeed = 1.25f;
    /**
	 * The target size of the view port.
	 */
    public Vector2 targetViewportSizeInPixels = new Vector2(1280.0f, 720.0f);
    /**
	 * Snap movement of the camera to pixels.
	 */
    public bool lockToPixels = true;
    /**
	 * The number of target pixels in every Unity unit.
	 */
    public float pixelsPerUnit = 32.0f;
    /**
	 * A game object that the camera will follow the x and y position of.
	 */

    public Vector2 climbDif;

    public Transform target;
    private Vector2 playerStart;
    private Vector3 cameraLocation;


    private Camera _camera;
    private int _currentScreenWidth = 0;
    private int _currentScreenHeight = 0;

    private float _pixelLockedPPU = 32.0f;
    private Vector2 _winSize;



    protected void Start()
    {
        //playerController = GetComponent<PlayerController>();
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 144;
        playerStart = new Vector3(target.position.x, target.position.y, target.position.z); // my code
        cameraLocation = new Vector3(target.position.x, target.position.y + 20, -20);
        _camera = this.GetComponent<Camera>();

        if (!_camera)
        {
            Debug.LogWarning("No camera for pixel perfect cam to use");
        }
        else
        {
            _camera.orthographic = true;
            // ResizeCamToTargetSize();
        }
    }

    public void ResizeCamToTargetSize()
    {
        if (_currentScreenWidth != Screen.width || _currentScreenHeight != Screen.height)
        {
            // check our target size here to see how much we want to scale this camera
            float percentageX = Screen.width / targetViewportSizeInPixels.x;
            float percentageY = Screen.height / targetViewportSizeInPixels.y;
            float targetSize = 0.0f;
            if (percentageX > percentageY)
            {
                targetSize = percentageY;
            }
            else
            {
                targetSize = percentageX;
            }
            int floored = Mathf.FloorToInt(targetSize);
            if (floored < 1)
            {
                floored = 1;
            }
            // now we have our percentage let's make the viewport scale to that
            float camSize = ((Screen.height / 2) / floored) / pixelsPerUnit;
            _camera.orthographicSize = camSize;
            _pixelLockedPPU = floored * pixelsPerUnit;
        }
        _winSize = new Vector2(Screen.width, Screen.height);
    }


    public void Update()
    {
        GetComponent<PlayerController>();
        isClimbingLedge = playerController.playerMovement.isClimbingLedge;

        Vector2 newPosition = new Vector2(target.position.x, target.position.y);
        float nextX = /*Mathf.Round*/(_pixelLockedPPU * newPosition.x);
        float nextY = /*Mathf.Round*/(_pixelLockedPPU * (newPosition.y));

       // CameraFollow(nextX, nextY);
    }

    public void LateUpdate()
    {


        /* if (_winSize.x != Screen.width || _winSize.y != Screen.height)
         {
             ResizeCamToTargetSize();
         }*/
        if (_camera && target)
        {

            Vector2 newPosition = new Vector2(target.position.x, target.position.y);
            float nextX = Mathf.Round(_pixelLockedPPU * newPosition.x);
            float nextY = Mathf.Round(_pixelLockedPPU * newPosition.y);

            if (!isClimbingLedge)

            {
                float xPos = (nextX / _pixelLockedPPU);
                float yPos = (nextY / _pixelLockedPPU) + 1;

                //float xPos = (newPosition.x);
                //float yPos = newPosition.y + 1;

                /* if (target.position.x < cameraLocation.x + 0.78f && //doesnt pass right
                     target.position.x > cameraLocation.x - 0.78f&& // doesnt pass left
                     target.position.y < cameraLocation.y + (verticalOffset - 1 )&& //doesnt pass top
                     target.position.y < cameraLocation.y - (verticalOffset + 1)) // doesnt pass bottom
                 { 
                     isInZone = true;
                 }*/


                if (target.position.x > cameraLocation.x + horizontalOffset && target.position.y > cameraLocation.y + (verticalOffset - 1))
                {
                    cameraAdjustment(xPos, horizontalOffset, -1, yPos, verticalOffset, -1);

                }

                else if (target.position.x > cameraLocation.x + horizontalOffset && target.position.y < cameraLocation.y - (verticalOffset + 1))
                {
                    cameraAdjustment(xPos, horizontalOffset, -1, yPos, verticalOffset, 1);
                }

                else if (target.position.x < cameraLocation.x - horizontalOffset && target.position.y > cameraLocation.y + (verticalOffset - 1))
                {
                    cameraAdjustment(xPos, horizontalOffset, 1, yPos, verticalOffset, -1);
                }

                else if (target.position.x < cameraLocation.x - horizontalOffset && target.position.y < cameraLocation.y - (verticalOffset + 1))
                {
                    cameraAdjustment(xPos, horizontalOffset, 1, yPos, verticalOffset, 1);
                }


                else if (target.position.x > cameraLocation.x + horizontalOffset) // follow if moving right
                {
                    cameraAdjustment(xPos, horizontalOffset, -1, cameraLocation.y, verticalOffset, 0);
                }

                else if (target.position.x < cameraLocation.x - horizontalOffset) // follows if moving left
                {
                    cameraAdjustment(xPos, horizontalOffset, 1, cameraLocation.y, verticalOffset, 0);
                }

                else if (target.position.y > cameraLocation.y + (verticalOffset - 1)) // follows in moving up
                {
                    cameraAdjustment(cameraLocation.x, horizontalOffset, 0, yPos, verticalOffset, -1);
                }

                else if (target.position.y < cameraLocation.y - (verticalOffset + 1)) // follows if moving down
                {
                    cameraAdjustment(cameraLocation.x, horizontalOffset, 0, yPos, verticalOffset, 1f);
                }
            }

            else if (isClimbingLedge)
            {
                CameraFollow(nextX, nextY);
            }
        }
    }

    public void CameraFollow(float nextX, float nextY)
    {
        if (isClimbingLedge && target.position.x < cameraLocation.x - (horizontalOffset) ||
            isClimbingLedge && target.position.x > cameraLocation.x + (horizontalOffset) ||
            isClimbingLedge && target.position.y > cameraLocation.y + (verticalOffset - 1) ||
            isClimbingLedge && target.position.y < cameraLocation.y - (verticalOffset + 1))
        {
            if (_camera && target)

                if (playerController.playerSurroundings.isOnPlatform)
                {
                    print("works");
                    if (playerController.playerMovement.isFacingRight)
                    {
                        cameraAdjustmentLedge(nextX, nextY, 0.5f);
                      //  Vector3 desiredPosition = new Vector3(nextX / _pixelLockedPPU + (0.5f), nextY / _pixelLockedPPU + 1.1f, -20);
                      //  climbDif = desiredPosition - cameraLocation;
                      //  _camera.transform.position = Vector3.Lerp(cameraLocation, desiredPosition, smoothSpeed * Time.deltaTime);
                      //  cameraLocation = _camera.transform.position;
                    }
                    else
                    {
                        cameraAdjustmentLedge(nextX, nextY, -0.5f);
                       // Vector3 desiredPosition = new Vector3(nextX / _pixelLockedPPU - (0.5f), nextY / _pixelLockedPPU + 1.1f, -20);
                       // climbDif = desiredPosition - cameraLocation;
                       // _camera.transform.position = Vector3.Lerp(cameraLocation, desiredPosition, smoothSpeed * Time.deltaTime);
                       // cameraLocation = _camera.transform.position;
                    }
                }

                else
                {
                    cameraAdjustmentLedge(nextX, nextY, 0);
                   // Vector3 desiredPosition = new Vector3(nextX / _pixelLockedPPU, nextY / _pixelLockedPPU + 1.1f, -20);
                   // climbDif = desiredPosition - cameraLocation;
                   // _camera.transform.position = Vector3.Lerp(cameraLocation, desiredPosition, smoothSpeed * Time.deltaTime);
                   // cameraLocation = _camera.transform.position;
                }
        }
        else
        {
            climbDif = new Vector2(0, 0);
        }
    }


    private void cameraAdjustment(float xLocation, float hOffset, float xDirection, float yLocation, float vOffset, float yDirection)
    {
        _camera.transform.position = new Vector3(xLocation + hOffset * xDirection, yLocation + vOffset * yDirection, -20);

        cameraLocation = _camera.transform.position;
        //isInZone = false;
    }

    private void cameraAdjustmentLedge(float nextX, float nextY, float offset)
    {
        Vector3 desiredPosition = new Vector3(nextX / _pixelLockedPPU + (offset), nextY / _pixelLockedPPU + 1.1f, -20);
        climbDif = desiredPosition - cameraLocation;
        _camera.transform.position = Vector3.Lerp(cameraLocation, desiredPosition, smoothSpeed * Time.deltaTime);
        cameraLocation = _camera.transform.position;
    }
}

