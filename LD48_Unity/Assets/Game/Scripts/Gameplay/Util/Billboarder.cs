using UnityEngine;

namespace LD48.Gameplay.Util
{
	public class Billboarder : MonoBehaviour
	{
		private Camera referenceCamera;

		public enum Axis
		{
			Up,
			Down,
			Left,
			Right,
			Forward,
			Back
		}

		public bool reverseFace;
		public Axis axis = Axis.Up;

		// return a direction based upon chosen axis
		private static Vector3 GetAxis(Axis refAxis)
		{
			return refAxis switch
			{
				Axis.Down => Vector3.down,
				Axis.Forward => Vector3.forward,
				Axis.Back => Vector3.back,
				Axis.Left => Vector3.left,
				Axis.Right => Vector3.right,
				// default is Vector3.up
				_ => Vector3.up
			};
		}

		private void Awake()
		{
			// if no camera referenced, grab the main camera
			if (referenceCamera == null)
			{
				referenceCamera = Camera.main;
			}
		}

		private void LateUpdate()
		{
			// rotates the object relative to the camera
			var cameraRotation = referenceCamera.transform.rotation;
			Vector3 targetPos = transform.position + cameraRotation * (reverseFace ? Vector3.forward : Vector3.back);
			Vector3 targetOrientation = cameraRotation * GetAxis(axis);
			transform.LookAt(targetPos, targetOrientation);
		}
	}
}