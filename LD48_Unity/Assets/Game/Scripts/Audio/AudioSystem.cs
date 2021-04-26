using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LD48.Audio
{
	public class AudioSystem : MonoBehaviour
	{
		[SerializeField]
		private AudioSource musicSource;
		public AudioSource MusicSource => musicSource;

		private float masterVolume = 1;
		private float musicVolume = 1;
		public float MusicVolume => musicVolume;
		private float audioVolume = 1;
		private float adjustedAudioVolume = 1;
		private bool isFading;

		public static AudioSystem Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				throw new Exception("There should only be a single instance of 'AudioSystem' active at one time.");
			}

			Instance = this;
		}

		public async void PlayMusic(AudioClip music)
		{
			await UniTask.WaitWhile(() => isFading);
			StartCoroutine(FadeIn(musicSource, 0.5f, PlayNewMusic));
			void PlayNewMusic()
			{
				musicSource.clip = music;
				musicSource.Play();
			}
		}

		public async void StopMusic()
		{
			await UniTask.WaitWhile(() => isFading);
			StartCoroutine(FadeOut(musicSource, 0.5f));
		}

		public async void PlaySound(AudioClip sound, bool loop)
		{
			var audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.loop = loop;
			audioSource.volume = adjustedAudioVolume;
			audioSource.clip = sound;
			audioSource.Play();
			if (!loop)
			{
				await UniTask.WaitWhile(() => audioSource.isPlaying);
				Destroy(audioSource);
			}
		}

		public void UpdateMusicVolume()
		{
			musicSource.volume = musicVolume * masterVolume;
		}

		public void AdjustMusicVolume(float value)
		{
			musicVolume = value;
			musicSource.volume = musicVolume * masterVolume;
		}

		public void AdjustAudioVolume(float value)
		{
			audioVolume = value;
			adjustedAudioVolume = value * masterVolume;
		}

		public void AdjustMasterVolume(float value)
		{
			masterVolume = value;
			AdjustAudioVolume(audioVolume);
			AdjustMusicVolume(musicVolume);
		}

		public IEnumerator FadeOut(AudioSource audioSource, float fadeTime, Action onComplete = null)
		{
			isFading = true;
			var startVolume = audioSource.volume;
			while (audioSource.volume > 0)
			{
				audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
				yield return null;
			}

			audioSource.Stop();
			isFading = false;
			onComplete?.Invoke();
		}

		public IEnumerator FadeIn(AudioSource audioSource, float fadeTime, Action onComplete = null)
		{
			isFading = true;
			audioSource.Play();
			audioSource.volume = 0f;
			while (audioSource.volume < musicVolume * masterVolume)
			{
				audioSource.volume += Time.deltaTime / fadeTime;
				yield return null;
			}

			isFading = false;
			onComplete?.Invoke();
		}
	}
}