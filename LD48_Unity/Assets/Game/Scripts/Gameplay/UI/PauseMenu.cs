using System;
using LD48.Core;
using LD48.Save;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using CharacterController = LD48.Gameplay.Player.CharacterController;

namespace LD48.Gameplay.UI
{
	public class PauseMenu : MonoBehaviour
	{
		[SerializeField]
		private Transform teleportLocation;

		[SerializeField]
		private CanvasGroup pauseCanvasGroup;

		private InputAction activateAction;
		private bool isSettingsOpen;
		private bool isPaused;
		private CursorLockMode previousCursorLockMode;
		private bool previousCursorVisible;

		private void Start()
		{
			pauseCanvasGroup.alpha = 0f;
			pauseCanvasGroup.interactable = false;
			pauseCanvasGroup.blocksRaycasts = false;

			var map = new InputActionMap("PauseMenu");
			activateAction = map.AddAction("activate", binding: "<Keyboard>/#(esc)");
			activateAction.AddBinding("<Keyboard>/#(enter)");

			activateAction.performed += HandleActivate;
			activateAction.Enable();
		}

		private void HandleActivate(InputAction.CallbackContext obj)
		{
			if (isSettingsOpen)
			{
				SettingsMenu.Instance.Deactivate();
			}
			else
			{
				if (isPaused)
				{
					Unpause();
				}
				else
				{
					Pause();
				}
			}
		}

		public void Pause()
		{
			if (GameCore.Instance.PlayerBusy) return;

			SaveData.Instance.SaveGame();
			isPaused = true;
			GameCore.Instance.PlayerBusy = true;
			Time.timeScale = 0f;
			pauseCanvasGroup.alpha = 1f;
			pauseCanvasGroup.interactable = true;
			pauseCanvasGroup.blocksRaycasts = true;

			previousCursorVisible = Cursor.visible;
			previousCursorLockMode = Cursor.lockState;

			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}

		public void Unpause()
		{
			SaveData.Instance.SaveGame();

			GameCore.Instance.PlayerBusy = false;
			isPaused = false;
			Time.timeScale = 1f;
			pauseCanvasGroup.alpha = 0f;
			pauseCanvasGroup.interactable = false;
			pauseCanvasGroup.blocksRaycasts = false;

			Cursor.lockState = previousCursorLockMode;
			Cursor.visible = previousCursorVisible;
		}

		public void Settings()
		{
			isSettingsOpen = true;
			SettingsMenu.Instance.Activate();
			SettingsMenu.Instance.OnDeactivate += HandleDeactivate;

			void HandleDeactivate()
			{
				isSettingsOpen = false;
			}
		}


		public async void ReturnToSub()
		{
			SaveData.Instance.SaveGame();

			pauseCanvasGroup.interactable = false;
			pauseCanvasGroup.blocksRaycasts = false;
			await GameCore.Instance.CameraFade.FadeTo(1f, 0.5f);
			CharacterController.Instance.Warp(teleportLocation.position, teleportLocation.rotation);
			Unpause();

			await GameCore.Instance.CameraFade.FadeTo(0f, 1f);
		}

		public async void ReturnToMainMenu()
		{
			SaveData.Instance.SaveGame();
			Unpause();
			await GameCore.Instance.CameraFade.FadeTo(1f, 0.5f);
			SceneManager.LoadScene("MainMenu");
			await SceneActivator.Run(1f);
		}

		private void OnDestroy()
		{
			activateAction.Dispose();
		}
	}
}