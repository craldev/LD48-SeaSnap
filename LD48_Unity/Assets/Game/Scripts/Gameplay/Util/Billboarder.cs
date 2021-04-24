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
		private Vector3 baseOffset;

		private void Start()
		{
			camera = Camera.main;
			baseOffset = transform.up;
		}

		private void Update()
		{
			var vector = camera.transform.forward;

			transform.LookAt(camera.transform.position, baseOffset);

		}
	}
}