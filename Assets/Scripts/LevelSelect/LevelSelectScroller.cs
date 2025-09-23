using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScroller : MonoBehaviour
{
    [SerializeField] Scrollbar lssb;
    [SerializeField] LevelSelectButtonManager lsbm;
    public bool menuOpen;
    [SerializeField] bool buttonHeld;
    [SerializeField] LevelSelectLauncherButton[] LevelSelectButtons;
    [Header("Objects needed to work")]
    [SerializeField] Camera cam;
    [SerializeField] public Transform LeftBound;
    [SerializeField] public Transform RightBound;
    [SerializeField] GameObject lookShooter;
    Vector2 OGTouchPos;
    Vector2 CurrentTouchPos;
    [Header("LookShooterStuff")]
    Vector3 OGWorldHitPos;
    Vector3 CurrentWorldHitPos;

    bool isBeingHeld;
    int beingHeldCounter;
    [SerializeField] int beingHeldThreshold;
    [SerializeField] float MaxCamSpeed;


    // Start is called before the first frame update
    void Start()
    {
        menuOpen = false;
        buttonHeld = false;
        isBeingHeld = false;
        //LevelSelectButtons = FindObjectsByType<LevelSelectLauncherButton>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DoButtonLogic();
        //some situations require the rest of the method to not run. These are them. The situations are: 
        //User holding a button, user not touching the screen, hold detection not yet being triggered, and user making invalid touches.

        //Having a menu open
        if (menuOpen) { return; }
        //holding a button
        if (buttonHeld) { return; }
        //not touching the screen
        if (Input.touchCount == 0)
        {
            ResetTouchData();
            return;
        }
        //Make sure that hold detection is being triggered.
        if (Input.touchCount > 0 & !isBeingHeld)
        {
            beingHeldCounter++;
            if (beingHeldCounter >= beingHeldThreshold)
            {
                isBeingHeld = true;
                //and finally, check for valid touches.
                RunValidTouchProcess();
            }
        }

        //We ONLY get here if all other requirements have been satisfied.
        if (isBeingHeld) { RunBeingHeld(); }
    }
    private void DoButtonLogic()
    {
        buttonHeld = false;
        if (LevelSelectButtons.Length == 0)
        {
            buttonHeld = false;
        }
        else
        {
            foreach (LevelSelectLauncherButton button in LevelSelectButtons)
            {
                if (buttonHeld) { break; }
                if (button.buttonPressed)
                {
                    buttonHeld = true;
                }
                else
                {
                    buttonHeld = false;
                }
            }
        }
    }
    private void RunValidTouchProcess()
    {

        Touch touch = Input.GetTouch(0);
        OGTouchPos = touch.position;
        CurrentTouchPos = touch.position;
        lookShooter.transform.localPosition = new Vector3(TouchPosToCanvasPosConverter(OGTouchPos.x), 0f, 0f);
        lookShooter.transform.LookAt(cam.transform);
        RaycastHit OGhit;
        if (Physics.Raycast(lookShooter.transform.position, -lookShooter.transform.forward, out OGhit, Mathf.Infinity))
        {
            //WHEN it collides with something...
            //Be sure to NEVER be able to look into the void.
            OGWorldHitPos = OGhit.point;
        }
        else
        {
            ResetTouchData();
        }
    }

    void RunBeingHeld()
    {
        if (cam.transform.position.x < LeftBound.position.x)
        {
            cam.transform.position = LeftBound.position;
            return;
        }
        if (cam.transform.position.x > RightBound.position.x)
        {
            cam.transform.position = RightBound.position;
            return;
        }
        Touch touch = Input.GetTouch(0);
        CurrentTouchPos = touch.position;
        lookShooter.transform.localPosition = new Vector3(TouchPosToCanvasPosConverter(touch.position.x), 0f, 0f);
        lookShooter.transform.LookAt(cam.transform);
        RaycastHit hit;
        if (Physics.Raycast(lookShooter.transform.position, -lookShooter.transform.forward, out hit, Mathf.Infinity))
        {
            CurrentWorldHitPos = hit.point;
            if (!Helper.isWithinMarginOfError(CurrentWorldHitPos, OGWorldHitPos, 1f))
            {
                RunCameraMover();
            }
        }
        else
        {
            //Reset the touch data if there's an issue with the incoming data from the hardware or from the user.
            ResetTouchData();
            return;
        }
    }

    void ResetTouchData()
    {
        beingHeldCounter = 0;
        isBeingHeld = false;
        OGTouchPos = Vector2.zero;
        CurrentTouchPos = Vector2.zero;
        OGWorldHitPos = Vector3.zero;
        CurrentWorldHitPos = Vector3.zero;
        lookShooter.transform.localPosition = Vector3.zero;
        lookShooter.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }


    float TouchPosToCanvasPosConverter(float touchX)
    {
        //Canvas X goes from -1280 to 1280
        //Touch X goes from 0 to 2560
        return Helper.RemapArbitraryValues(0f, 2560f, -1280f, 1280f, touchX);
    }
    float CanvasPosToTouchPosConverter(float canvasX)
    {
        //Canvas X goes from -1280 to 1280
        //Touch X goes from 0 to 2560
        return Helper.RemapArbitraryValues(-1280f, 1280f, 0f, 2560f, canvasX);
    }
    void RunCameraMover()
    {
        float touchPosDiff = OGTouchPos.x - CurrentTouchPos.x;

        //If it's moving towards the left bound and is close to it, slow down.
        if (touchPosDiff < 0 & Helper.isWithinMarginOfError(cam.transform.position, LeftBound.position, 10.0f))
        {
            touchPosDiff = touchPosDiff * Helper.RemapToBetweenZeroAndOne(0, 10, Vector3.Distance(cam.transform.position, LeftBound.transform.position));
        }
        //If it's moving towards the right bound and is too close to it, slow down.
        if (touchPosDiff > 0 & Helper.isWithinMarginOfError(cam.transform.position, RightBound.position, 10.0f))
        {
            touchPosDiff = touchPosDiff * Helper.RemapToBetweenZeroAndOne(0, 10, Vector3.Distance(cam.transform.position, RightBound.transform.position));
        }
        //Move the camera by the amount we figured out, capped by the max camera speed.
        float xval = cam.transform.position.x + Helper.RemapArbitraryValues(0f, 1280f, 0f, MaxCamSpeed, touchPosDiff);
        lssb.value = Helper.RemapToBetweenZeroAndOne(LeftBound.position.x, RightBound.position.x, xval);
        lsbm.SetLevelScroll();

    }

    public void SetLeftRightScrollAmount(float scrollPercent)
    {
        float goalPos = Helper.RemapArbitraryValues(0f, 1f, LeftBound.position.x, RightBound.position.x, scrollPercent); //should remap 0-1 to xmin and max.
        cam.transform.position = new Vector3(goalPos, cam.transform.position.y, cam.transform.position.z);
    }
    public float GetLeftRightScrollAmount()
    {
        return Helper.RemapToBetweenZeroAndOne(LeftBound.position.x, RightBound.position.x,cam.transform.position.x);  
    }
}
