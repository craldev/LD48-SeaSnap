using System;
using UnityEngine;

namespace LD48.Gameplay.Util
{
	public class Billboarder : MonoBehaviour
	{
		[Flags]
		private enum Axis
		{
			X = 1,
			Y = 2,
			Z = 4
		}
		[SerializeField]
		private SpriteRenderer renderer;

		[SerializeField]
		private Axis lockedAxis;

		private UnityEngine.Camera camera;

		private void Start()
		{
			if (camera == null)
			{
				camera = UnityEngine.Camera.main;
			}
		}

		private void Update()
		{
			var position = transform.position;
			var cameraDirection = (position - camera.transform.position).normalized;

			if (lockedAxis.HasFlag(Axis.X))
			{
				cameraDirection.x = 0f;
			}

			if (lockedAxis.HasFlag(Axis.Y))
			{
				cameraDirection.y = 0f;
			}

			if (lockedAxis.HasFlag(Axis.Z))
			{
				cameraDirection.z = 0f;
			}

			renderer.transform.localRotation = Quaternion.LookRotation(cameraDirection);
		}
	}
}