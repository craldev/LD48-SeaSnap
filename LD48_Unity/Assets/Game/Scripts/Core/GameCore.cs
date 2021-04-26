using System;
using UnityEngine;

namespace LD48.Core
{
	public class GameCore : MonoBehaviour
	{
		public static GameCore Instance { get; private set; }

		[SerializeField]
		private CameraFade cameraFade;
		public CameraFade CameraFade => cameraFade;

		[SerializeField]
		private EntityLibrary entityLibrary;
		public EntityLibrary EntityLibrary => entityLibrary;

		public bool PlayerBusy { get; set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				throw new Exception("[GameCore] Only one GameCore can be active in a scene at the same time.");
			}

			DontDestroyOnLoad(this);

			EntityLibrary.Initialize();
		}
	}
}