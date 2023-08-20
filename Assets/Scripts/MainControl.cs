using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class MainControl : MonoBehaviour
{
    public MainUI ui;
    public Checkpoints checkpoints;
    public static MainControl l;
    private void Awake()
    {
        l = this;
    }
    private void Start()
    {
        PlayerControl.l.EnableThis(false);
        ui.navigationMarker.gameObject.SetActive(false);
    }

    private void Update()
    {
        #region Checking for the player reaching the next checkpoint
        if (checkpoints.target == null)
            return;
        // sqrMagnitude is used instead of magnitude because its cheaper to do the comparison
        if ((PlayerControl.l.transform.position - checkpoints.target.transform.position).sqrMagnitude < Checkpoints.minSqrDistToCheckTarget)
        {
            Debug.Log("Reached checkpoint " + checkpoints.target.index);
            checkpoints.ReachedTarget();
        }
        /// debugging
        //else
        //{
        //    Debug.Log($"{(PlayerControl.l.transform.position - checkpoints.target.transform.position).sqrMagnitude} < {Checkpoints.minSqrDistToCheckTarget}");
        //}
        #endregion
    }

    #region Functions

    /// <summary> Called from the button event in the scene </summary>
    public void StartGame()
    {
        Checkpoints.l.IndexCheckpoints();
        // targeting the first checkpoint
        checkpoints.TargetCheckpoint(checkpoints.checkpoints[0]);
        PlayerControl.l.EnableThis(true);
        ui.startButton.SetActive(false);
        ui.navigationMarker.gameObject.SetActive(true);
    }

    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    #endregion
}
