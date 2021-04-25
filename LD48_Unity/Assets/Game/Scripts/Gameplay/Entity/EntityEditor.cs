#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace LD48.Gameplay.Entity
{
	[CustomEditor(typeof(Entity))]
	public class EntityEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var script = (Entity)target;

			if(GUILayout.Button("Generate New GUID", GUILayout.Height(40)))
			{
				EditorUtility.SetDirty(script);
				script.GUID = Guid.NewGuid().ToString();
			}
		}
	}
}
#endif