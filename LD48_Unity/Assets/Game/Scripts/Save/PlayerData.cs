using System;
using UnityEngine;

namespace LD48.Save
{
	[Serializable]
	public class PlayerData
	{
		[SerializeField]
		private string currentScene;
		public string CurrentScene { get => currentScene; set => currentScene = value; }

		[SerializeField]
		private Vector3 playerPosition;
		public Vector3 PlayerPosition { get => playerPosition; set => playerPosition = value; }

		[SerializeField]
		private Quaternion playerRotation;
		public Quaternion PlayerRotation { get => playerRotation; set => playerRotation = value; }

		[SerializeField]
		private Vector3 cameraPosition;
		public Vector3 CameraPosition { get => cameraPosition; set => cameraPosition = value; }

		[SerializeField]
		private float cameraYValue;
		public float CameraYValue { get => cameraYValue; set => cameraYValue = value; }
	}
}