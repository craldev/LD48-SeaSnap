using UnityEngine;

namespace LD48.Gameplay.Player
{
	public class CursorControl : MonoBehaviour
	{
		private void Awake()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}