using LD48.Save;
using UnityEngine;

namespace LD48.Core
{
	public class AutoStarter : MonoBehaviour
	{
		private const string CORE_PATH = "GameCore";

		[SerializeField]
		private string loadSavePath;

		[SerializeField]
		private SaveData saveData;

		private async void Awake()
		{
			if (GameCore.Instance == null)
			{
				var core = LoadGameCore();
				core = Instantiate(core);

				if (!string.IsNullOrEmpty(loadSavePath))
				{
					var result = SaveData.Instance.LoadGame(loadSavePath);

					Debug.Log("Loading Save Data " + result);
				}
				else
				{
					SaveData.Instance.NewGame("Game1");
				}
				saveData = SaveData.Instance;

				await core.CameraFade.FadeTo(0f);
				await SceneActivator.Run(0.5f);
			}
		}

		private static GameCore LoadGameCore()
		{
			var request = Resources.LoadAsync<GameCore>(CORE_PATH);
			return request.asset as GameCore;
		}
	}
}