using System;
using UnityEngine;

namespace LD48.Save
{
	[Serializable]
	public class UpgradeData
	{
		[SerializeField]
		private int researchCurrency;
		public int ResearchCurrency => researchCurrency;

		[SerializeField]
		private bool scannerUpgrade;
		public bool ScannerUpgrade { get => scannerUpgrade; set => scannerUpgrade = value; }

		[SerializeField]
		private int depthVision;
		public int DepthVision { get => depthVision; set => depthVision = value; }

		[SerializeField]
		private int swimSpeed;
		public int SwimSpeed { get => swimSpeed; set => swimSpeed = value; }

		public Action<int, int> OnCurrencyAdded { get; set; }
		public Action<int, int> OnCurrencyRemoved { get; set; }

		public void AddCurrency(int value)
		{
			researchCurrency += value;
			OnCurrencyAdded?.Invoke(researchCurrency, value);
		}

		public void RemoveCurrency(int value)
		{
			researchCurrency -= value;
			OnCurrencyRemoved?.Invoke(researchCurrency, value);
		}
	}
}