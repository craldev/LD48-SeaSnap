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
	}
}