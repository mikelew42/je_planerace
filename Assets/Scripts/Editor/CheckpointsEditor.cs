using NUnit.Framework;
using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopDownRPGDemo
{
    [CustomEditor(typeof(Checkpoints))]
    public class CheckpointsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // this runs the base code that this overrides
            base.OnInspectorGUI();

            /*
            if (GUILayout.Button("Index Checkpoints"))
            {
                IndexCheckpoints((Checkpoints)target);
            }
            */
        }

        /*
        public void IndexCheckpoints(Checkpoints target)
        {
            List<Checkpoint> checkpoints = new List<Checkpoint>();
            for (int i = 0; i < target.transform.childCount; i++)
            {
                checkpoints.Add(target.transform.GetChild(i).GetComponent<Checkpoint>());
                checkpoints[checkpoints.Count - 1].index = i;
                EditorUtility.SetDirty(checkpoints[checkpoints.Count - 1]);
            }
            target.checkpoints = checkpoints.ToArray();
            EditorUtility.SetDirty(target);
        }
        */

    }
}