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

		public async void NewGame()
		{
			//New Save
			SaveData.Instance.NewGame("Game1");

			//Load Scene
			await GameCore.Instance.CameraFade.FadeTo(1f, 0.5f);
			SceneManager.LoadScene(gameSceneName);
			await SceneActivator.Run(1f);
		}

		public async void LoadGame()
        {
        	//Load Save
        	SaveData.Instance.LoadGame(SaveFilePath);

        	//Load Scene
        	await GameCore.Instance.CameraFade.FadeTo(1f, 0.5f);
        	await SceneManager.LoadSceneAsync(gameSceneName);
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