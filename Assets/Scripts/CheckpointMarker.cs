using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CheckpointMarker : MonoBehaviour
{
    public static CheckpointMarker l;
    public Image markerImg;
    public RectTransform marker;
    public Camera mainCamera;
    public float canvasScale = 1.0f; // The desired canvas scale
    public TextMeshProUGUI distanceText;
    private Transform playerTransform;

    private void Awake()
    {
        l = this;
    }

    private void Start()
    {
        mainCamera = PlayerControl.l.mainCamera;
        playerTransform = PlayerControl.l.transform;
    }

    // This update is only for moving the navigation marker around the screen and showing the meters until reaching the target
    private void Update()
    {
        if (MainControl.l.checkpoints.target == null)
            return;

        #region Moving the marker around the UI to show where to go

        float minX = markerImg.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = markerImg.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = mainCamera.WorldToScreenPoint(MainControl.l.checkpoints.target.transform.position);

        if (Vector3.Dot(MainControl.l.checkpoints.target.transform.position -
            PlayerControl.l.transform.position,
            PlayerControl.l.transform.forward) < 0)
        {
            // target is behind the player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        markerImg.transform.position = pos;

        #endregion

        // Set the UI to show the distance to reach the target
        distanceText.text = Mathf.RoundToInt(Vector3.Distance(PlayerControl.l.transform.position, Checkpoints.l.target.transform.position)).ToString() + " m";
    }
}
