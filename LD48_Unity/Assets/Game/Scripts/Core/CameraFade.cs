using System;
using Cysharp.Threading.Tasks;
using DUCK.Tween;
using DUCK.Tween.Easings;
using DUCK.Tween.Extensions;
using UnityEngine;

namespace LD48.Core
{
	public class CameraFade : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		private UIFadeAnimation fadeAnimation;

		private void Awake()
		{
			FadeTo(0f);
		}

		public async UniTask FadeTo(float alpha, float duration = 0f)
		{
			if (fadeAnimation != null)
			{
				await UniTask.WaitWhile(() => fadeAnimation.IsPlaying);
			}

			var canvasGroupAlpha = canvasGroup.alpha;
			fadeAnimation = canvasGroup.Fade(canvasGroupAlpha, alpha,  duration * Mathf.Abs(canvasGroupAlpha - alpha), Ease.Quad.InOut);

			if (alpha > 0)
			{
				canvasGroup.interactable = true;
				canvasGroup.blocksRaycasts = true;
			}

			fadeAnimation.Play();
			await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.UnscaledDeltaTime);
			if (alpha <= 0)
			{
				canvasGroup.interactable = false;
				canvasGroup.blocksRaycasts = false;
			}
		}
	}
}