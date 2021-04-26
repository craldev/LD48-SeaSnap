using System;
using Cysharp.Threading.Tasks;
using LD48.Core;
using LD48.Save;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD48.Gameplay.UI
{
	public class MainMenu : MonoBehaviour
	{
		private string SaveFilePath => $"{Application.persistentDataPath}/Game1/save.json";

		[SerializeField]
		private string gameSceneName;

		private void Awake()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
			SaveData.Instance.Reset();
		}

		public async void NewGame()
		{
			//New Save
			SaveData.Instance.NewGame("Game1");

			//Load Scene
			await GameCore.Instance.CameraFade.FadeTo(1f, 0.5f);
			Debug.Log("New Game Start");
			await SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Single);
			await UniTask.Delay(TimeSpan.FromSeconds(1f));
			Debug.Log("New Game Done");
			await GameCore.Instance.CameraFade.FadeTo(0f, 0.5f);
		}

		public async void LoadGame()
        {
        	//Load Save
        	SaveData.Instance.LoadGame(SaveFilePath);

        	//Load Scene
        	await GameCore.Instance.CameraFade.FadeTo(1f, 0.5f);
        	await SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Single);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        	await SceneActivator.Run(1f);
        }

		public void Settings()
		{
			SettingsMenu.Instance.Activate();
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}