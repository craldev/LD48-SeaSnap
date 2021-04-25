using UnityEngine;

namespace LD48.Gameplay.Player
{
	public class CursorControl : MonoBehaviour
	{
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
		}
	}
}