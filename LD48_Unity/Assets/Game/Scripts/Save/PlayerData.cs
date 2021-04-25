using System;
using UnityEngine;

namespace LD48.Save
{
	[Serializable]
	public class PlayerData
	{
		[SerializeField]
		private Vector3 playerPosition;
		public Vector3 PlayerPosition { get => playerPosition; set => playerPosition = value; }

		[SerializeField]
		private Quaternion playerRotation;
		public Quaternion PlayerRotation { get => playerRotation; set => playerRotation = value; }

		[SerializeField]
		private Quaternion cameraRotation;
		public Quaternion CameraRotation { get => cameraRotation; set => cameraRotation = value; }

	}
}