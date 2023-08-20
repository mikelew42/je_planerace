using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public enum Position { Standard, Tunnel }
    
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 camOffset = new Vector3(0, 5, -10);
    private static readonly Vector3 standardCameraOffset = new Vector3(0, 10, -18);
    /// <summary> The camera offset to be used when flying through tunnels </summary>
    private static readonly Vector3 tunnelCameraOffset = new Vector3(0, 1, -5);
    public float smoothTime = 0.3F;
    //public float rotationSmoothTime = 0.1F;
    public float rotationSmoothSpeed = 0.5F;
    private Vector3 velocity = Vector3.zero;
    private float lastTimeCameraCollided;
    private bool insideTunnelDetected;
    private static readonly float timeBetweenTunnelChecks = 1f;
    private float nextTunnelCheckAtTime;
    private static readonly float radiusForTunnelChecks = 30f;

    public LayerMask environmentLayers;

    private void Start()
    {
        SetCameraOffset(Position.Standard);
    }

    private void FixedUpdate()
    {
        #region Camera movement

        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(camOffset);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        #endregion

        #region Camera rotation

        // Smoothly rotate the camera to match the target's rotation
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.fixedDeltaTime);

        #endregion

        #region Changing the camera offset when inside an enclosed space
        if (Time.time > nextTunnelCheckAtTime)
        {
            nextTunnelCheckAtTime = Time.time + timeBetweenTunnelChecks;

            // checking for colliders with the environment layers
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusForTunnelChecks, environmentLayers);
            if (hitColliders.Length > 0)
            {
                if (!insideTunnelDetected)
                {
                    Debug.Log("Inside tunnel started");
                    insideTunnelDetected = true;
                    SetCameraOffset(Position.Tunnel);
                }
            }
            else
            {
                if (insideTunnelDetected)
                {
                    Debug.Log("Inside tunnel ended");
                    insideTunnelDetected = false;
                    SetCameraOffset(Position.Standard);
                }
            }
        }
        #endregion
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            return;
        lastTimeCameraCollided = Time.time;
    }


    #region Functions
    public void SetCameraOffset(Position position)
    {
        switch (position)
        {
            case Position.Standard:
                camOffset = standardCameraOffset;
                break;
            case Position.Tunnel:
                camOffset = tunnelCameraOffset;
                break;
            default:
                Debug.LogError("No case for " + position);
                break;
        }
    }

    #endregion
}