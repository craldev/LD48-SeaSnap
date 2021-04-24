using System;
using UnityEngine;

namespace LD48.Gameplay.Util
{
	public class Billboarder : MonoBehaviour
	{
		[Flags]
		private enum Axis
		{
			X = 1 << 0,
			Y = 1 << 1,
			Z = 1 << 2
		}

		[SerializeField]
		private Axis axis;

		private Camera camera;

		private void Start()
		{
			camera = Camera.main;
		}

		private void Update()
		{
			var cameraPosition = camera.transform.position;
			var v = cameraPosition - transform.position;

			if (axis.HasFlag(Axis.X))
			{
				v.x = 0f;
			}

			if (axis.HasFlag(Axis.Y))
			{
				v.y = 0f;
			}

			if (axis.HasFlag(Axis.Z))
			{
				v.z = 0f;
			}

			transform.LookAt(cameraPosition - v);
		}
	}
}