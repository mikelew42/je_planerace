using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public CheckpointMarker checkpointMarker;
    public Checkpoint[] checkpoints;
    public static Checkpoints l;
    public Checkpoint target;
    /// <summary> The squared distance that will determine when the player reaches a checkpoint (for 50 use 2500) </summary>
    public static readonly float minSqrDistToCheckTarget = 10000;

    private void Awake()
    {
        l = this;
    }

    // Draws the lines and spheres in the scene over the checkpoint path when selecting the checkpoints parent object
    private void OnDrawGizmosSelected()
    {
        if (transform.childCount < 2)
            return;
        Gizmos.DrawSphere(transform.GetChild(0).position, 3);
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawSphere(transform.GetChild(i + 1).position, 3);
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }

    /// <summary> Reached the target checkpoint, targeting the next one or turning off navigation system if path is finished  </summary>
    public void ReachedTarget()
    {
        if (target.index == (checkpoints.Length - 1))
        {
            TargetCheckpoint(null);
            return;
        }
        TargetCheckpoint(checkpoints[target.index + 1]);
    }

    /// <summary> Sets an index to each checkpoint </summary>
    public void IndexCheckpoints()
    {
        List<Checkpoint> checkpoints = new List<Checkpoint>();
        for (int i = 0; i < transform.childCount; i++)
        {
            checkpoints.Add(transform.GetChild(i).GetComponent<Checkpoint>());
            checkpoints[checkpoints.Count - 1].index = i;
        }
        this.checkpoints = checkpoints.ToArray();
    }

    /// <summary> Sets the input checkpoint to be the player's current target, or input null to turn the checkpoint marker off </summary>
    public void TargetCheckpoint(Checkpoint checkpoint)
    {
        target = checkpoint;
        // targeting null will turn the checkpoint marker off
        if (checkpoint == null)
        {
            checkpointMarker.gameObject.SetActive(false);

            // for now just restarting the game once the final target is reached
            MainControl.l.ReloadScene();
            return;
        }
        // making sure the checkpoint marker is on
        if (!checkpointMarker.gameObject.activeSelf)
        {
            checkpointMarker.gameObject.SetActive(true);
        }
    }
}
