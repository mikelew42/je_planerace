using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int index;
    public Checkpoints main;

    // Draws the lines and spheres in the editor scene over the checkpoint path when selecting a checkpoint
    private void OnDrawGizmosSelected()
    {
        if (main.transform.childCount < 2)
            return;
        Gizmos.DrawSphere(main.transform.GetChild(0).position, 3);
        for (int i = 0; i < main.transform.childCount - 1; i++)
        {
            Gizmos.DrawSphere(main.transform.GetChild(i + 1).position, 3);
            Gizmos.DrawLine(main.transform.GetChild(i).position, main.transform.GetChild(i + 1).position);
        }
    }
}
