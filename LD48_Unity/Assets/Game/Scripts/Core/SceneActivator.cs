using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LD48.Core
{
	public static class SceneActivator
	{
		public static async UniTask Run(float fadeDuration)
		{
			var sceneInitializedTypes = FindOfType<ISceneInitialized>();
			var sceneLoadedTypes = FindOfType<ISceneLoaded>();

			foreach (var sceneInitializedType in sceneInitializedTypes)
			{
				sceneInitializedType.HandleSceneInitialized();
			}

			await GameCore.Instance.CameraFade.FadeTo(0f, fadeDuration);

			foreach (var sceneLoadedType in sceneLoadedTypes)
			{
				sceneLoadedType.HandleSceneLoaded();
			}
		}

		private static List<T> FindOfType<T>() where T : class
		{
			return new List<T>(Object.FindObjectsOfType<MonoBehaviour>().OfType<T>().ToList());
		}
	}

	/// <summary>
	/// This is called during the loading screen.
	/// </summary>
	public interface ISceneInitialized
	{
		void HandleSceneInitialized();
	}

	/// <summary>
	/// This is called after the loading screen.
	/// </summary>
	public interface ISceneLoaded
	{
		void HandleSceneLoaded();
	}
}