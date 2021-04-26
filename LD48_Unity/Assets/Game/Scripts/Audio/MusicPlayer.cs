using UnityEngine;

namespace LD48.Audio
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField]
		private AudioClip music;

		private void Start()
		{
			AudioSystem.Instance.PlayMusic(music);
		}
	}
}