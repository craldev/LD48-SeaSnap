using DUCK.Tween;
using DUCK.Tween.Extensions;
using LD48.Save;
using TMPro;
using UnityEngine;

namespace LD48.Gameplay.UI
{
	public class CurrencyCounter : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private TextMeshProUGUI valueText;

		[SerializeField]
		private TextMeshProUGUI floatingText;

		[SerializeField]
		private Color addColor = Color.green;

		[SerializeField]
		private Color removeColor = Color.red;

		[SerializeField]
		private bool disableFade;

		private ParalleledAnimation floatingAnimation;
		private SequencedAnimation fadeAnimation;

		public void Start()
		{
			SaveData.Instance.UpgradeData.OnCurrencyAdded += HandleCurrencyAdded;
			SaveData.Instance.UpgradeData.OnCurrencyRemoved += HandleCurrencyRemoved;
			floatingAnimation = new ParalleledAnimation();
			floatingAnimation.MoveLocal(floatingText.transform, new Vector3(-90f, 45f, 0f), new Vector3(-90f, 90f, 0f));

			floatingAnimation.Custom(SetTextAlpha, 1f, 0f);


			if (!disableFade)
			{
				Debug.Log(gameObject.name);
				fadeAnimation = new SequencedAnimation();
				fadeAnimation.Fade(canvasGroup, 0f, 1f, 0f);
				fadeAnimation.Wait(5f);
				fadeAnimation.Fade(canvasGroup, 1f, 0f);
				canvasGroup.alpha = 0f;
			}

			valueText.text = SaveData.Instance.UpgradeData.ResearchCurrency.ToString();
			floatingText.alpha = 0f;
			if (!disableFade)
			{
				SetTextAlpha(0f);
			}
		}

		private void HandleCurrencyAdded(int total, int value)
		{
			floatingText.color = addColor;
			floatingAnimation.Abort();
			valueText.text = total.ToString();

			floatingText.text = "+" + value;
			SetTextAlpha(1f);
			floatingAnimation.Play();

			if (fadeAnimation != null)
			{
				fadeAnimation.Abort();
				fadeAnimation.Play();
			}
		}

		private void HandleCurrencyRemoved(int total, int value)
		{
			floatingText.color = removeColor;
			floatingAnimation.Abort();
			valueText.text = total.ToString();

			floatingText.text = "-" + value;
			SetTextAlpha(1f);
			floatingAnimation.Play();

			if (fadeAnimation != null)
			{
				fadeAnimation.Abort();
				fadeAnimation.Play();
			}
		}


		private void SetTextAlpha(float value)
		{
			floatingText.alpha = value;
		}
	}
}