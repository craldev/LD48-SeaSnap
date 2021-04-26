using UnityEngine;

namespace LD48.Gameplay.UI
{
	public class SettingsMenu : MonoBehaviour
	{
		public static SettingsMenu Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
		}

		public void Activate()
		{

		}

		public void Deactivate()
		{

		}
	}
}