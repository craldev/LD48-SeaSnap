using LD48.Audio;
using UnityEngine;

namespace LD48.Utils
{
	public class PlaySound : MonoBehaviour
	{
		[SerializeField]
		private AudioClip sound;

		private void Start()
		{
			AudioSystem.Instance.PlaySound(sound, true);
		}
	}
}