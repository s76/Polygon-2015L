using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Train))]
public class TrainEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		
		Train myTarget = (Train)target;
		if(GUILayout.Button("Replace parts"))
		{
			myTarget.ReplaceParts();
		}
	}
}

