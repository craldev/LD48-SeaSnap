using System;
using LD48.Audio;
using LD48.Core;
using UnityEngine;
using UnityEngine.UI;
using CharacterController = LD48.Gameplay.Player.CharacterController;

namespace LD48.Gameplay.UI
{
	public class SettingsMenu : MonoBehaviour, ISceneLoaded
	{
		public static SettingsMenu Instance { get; private set; }

		[SerializeField]
		private CanvasGroup settingsMenu;

		[SerializeField]
		private Slider masterVolume;

		[SerializeField]
		private Slider musicVolume;

		[SerializeField]
		private Slider audioVolume;

		[SerializeField]
		private Slider mouseSens;

		public Action OnDeactivate { get; set; }

		private CursorLockMode previousCursorLockMode;
		private bool previousCursorVisible;

		private void Awake()
		{
			settingsMenu.alpha = 0f;
			settingsMenu.interactable = false;
			settingsMenu.blocksRaycasts = false;

			Instance = this;
		}

		private void Start()
		{
			Load();
		}

		public void Save()
		{
			PlayerPrefs.SetFloat("Music", musicVolume.value);
			PlayerPrefs.SetFloat("Audio", audioVolume.value);
			PlayerPrefs.SetFloat("Master", masterVolume.value);
			PlayerPrefs.SetFloat("MouseSens", mouseSens.value);
		}

		public void Load()
		{
			if (PlayerPrefs.HasKey("Master"))
			{
				musicVolume.value = PlayerPrefs.GetFloat("Music");
				audioVolume.value = PlayerPrefs.GetFloat("Audio");
				masterVolume.value = PlayerPrefs.GetFloat("Master");

				if (PlayerPrefs.HasKey("MouseSens"))
				{
					mouseSens.value = PlayerPrefs.GetFloat("MouseSens");
					MouseSensChanged();
				}

				MasterVolumeChanged();
				MusicVolumeChanged();
				AudioVolumeChanged();
			}
		}

		public void MasterVolumeChanged()
		{
			AudioSystem.Instance.AdjustMasterVolume(masterVolume.value);
		}

		public void MusicVolumeChanged()
		{
			AudioSystem.Instance.AdjustMusicVolume(musicVolume.value);
		}

		public void AudioVolumeChanged()
		{
			AudioSystem.Instance.AdjustAudioVolume(audioVolume.value);
		}

		public void MouseSensChanged()
		{
			CharacterController.Instance.UpdateSens(mouseSens.value);
		}

		public void WindowedActivate()
		{
			Screen.fullScreenMode = FullScreenMode.Windowed;
			Screen.SetResolution (1280, 720, false);
		}

		public void FullscreenActivate()
		{
			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
			Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, true);
		}

		public void BorderlessActivate()
		{
			Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
			Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, true);
		}

		public void Activate()
		{
			settingsMenu.alpha = 1f;
			settingsMenu.interactable = true;
			settingsMenu.blocksRaycasts = true;

			previousCursorVisible = Cursor.visible;
			previousCursorLockMode = Cursor.lockState;

			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}

		public void Deactivate()
		{
			Save();
			settingsMenu.alpha = 0f;
			settingsMenu.interactable = false;
			settingsMenu.blocksRaycasts = false;

			Cursor.lockState = previousCursorLockMode;
			Cursor.visible = previousCursorVisible;

			OnDeactivate?.Invoke();
		}

		public void HandleSceneLoaded()
		{
			Load();
		}
	}
}