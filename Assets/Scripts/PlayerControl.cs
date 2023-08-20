using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    public Camera mainCamera;
    public PlaneMovement movement;
    public InputAction move;
    public InputAction look;
    public InputRig inputRig;
    public static PlayerControl l;
    public GameObject model;
    public Vector2 startTouchPosition;
    private static readonly float mobileControlSensitivity = 20f;
    private static readonly float mobileYawSensitivity = 0.15f;
    private bool fire1KeyDown;
    private bool fire1KeyUp;
    private bool swipeStarted;

    private void Awake()
    {
        l = this;
    }

    private void Start()
    {
#if UNITY_EDITOR_WIN
        inputRig = new InputRig();
        move = inputRig.PlaneControl.Move;
        move.Enable();
        look = inputRig.PlaneControl.Look;
        look.Enable();
#elif UNITY_ANDROID || UNITY_IOS
        
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR_WIN

        #region Simulated phone touch movement
        if (fire1KeyDown)
        {
            swipeStarted = true;
            startTouchPosition = Input.mousePosition;
        }
        if (swipeStarted)
        {
            if (fire1KeyUp)
            {
                movement.HandeInputs(((Vector2)Input.mousePosition - startTouchPosition).normalized * Time.deltaTime * mobileControlSensitivity, 0);
                swipeStarted = false;
            }
            else
            {
                movement.HandeInputs(((Vector2)Input.mousePosition - startTouchPosition).normalized * Time.deltaTime * mobileControlSensitivity, 0);
            }
        }
        else
        {
            movement.HandeInputs(Vector2.zero, 0);
        }
        ResetInput();
        #endregion

        #region WASD Controls which still works on top of the simulated phone touch movement
        if (move.ReadValue<Vector2>().sqrMagnitude > 0.2 || look.ReadValue<Vector2>().sqrMagnitude > 0.2f)
            movement.HandeInputs(move.ReadValue<Vector2>(), look.ReadValue<Vector2>().x);
        #endregion

#elif UNITY_ANDROID || UNITY_IOS

        #region (REMOVED, yaw control with 2 fingers)
        /*
        // two finger input for yaw
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended)
            {
                movement.HandeInputs(Vector2.zero, (Input.GetTouch(0).position.x - startTouchPosition.x) * Time.deltaTime * mobileYawSensitivity);
            }
            else
            {
                movement.HandeInputs(Vector2.zero, (Input.GetTouch(0).position.x - startTouchPosition.x) * Time.deltaTime * mobileYawSensitivity);
            }
        }
        */
        #endregion

        // 1 or 3+ finger input for roll and pitch
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended)
            {
                movement.HandeInputs((Input.GetTouch(0).position - startTouchPosition).normalized * Time.deltaTime * mobileControlSensitivity, 0);
            }
            else
            {
                movement.HandeInputs((Input.GetTouch(0).position - startTouchPosition).normalized * Time.deltaTime * mobileControlSensitivity, 0);
            }
        }
        else
        {
            movement.HandeInputs(Vector2.zero, 0);
        }
#endif

    }

    public void ResetInput()
    {
        fire1KeyDown = false;
        fire1KeyUp = false;
    }

    #region Input actions

    // this is called from the Player gameObject, Player Input script, open up 'Events' and 'Plane Control'
    public void Fire1Action(InputAction.CallbackContext context)
    {
        Debug.Log("Input: Device: " + context.control.device.displayName);
        if (context.started)
        {
            fire1KeyDown = true;
        }
        else if (context.canceled)
        {
            fire1KeyUp = true;
        }
    }

    #endregion

    public void EnableThis(bool enable)
    {
        movement.enabled = enable;
        enabled = enable;
        movement.rb.isKinematic = !enable;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            return;
        // if there is any non-player collision, just destroy the plane and restart the level for now
        Destroyed();
    }

    /// <summary> Disabled the plane and restarts the game after 2 seconds </summary>
    public void Destroyed()
    {
        model.gameObject.SetActive(false);
        EnableThis(false);
        Invoke("DestroyedDone", 2);
    }

    public void DestroyedDone()
    {
        MainControl.l.ReloadScene();
    }
}
