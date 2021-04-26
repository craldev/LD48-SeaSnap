using System;
using LD48.Core;
using LD48.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD48.Gameplay.UI
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Upgrade[] upgrades;

        [SerializeField]
        private TextMeshProUGUI actionText;

        [SerializeField]
        private Button buttonTemplate;

        [SerializeField]
        private Transform parent;

        private bool isActive;
        public Action OnDeactivate { get; set; }

        public void Start()
        {
            canvasGroup.alpha = 0f;
            actionText.text = "";

            foreach (var upgrade in upgrades)
            {
                var newButton = Instantiate(buttonTemplate, parent);

                var text = newButton.GetComponentInChildren<TextMeshProUGUI>();
                text.text = upgrade.upgradeName + ": " + upgrade.cost;
                newButton.onClick.AddListener(()=> HandleClick(newButton, upgrade));

                switch (upgrade.upgradeType)
                {
                    case Upgrade.UpgradeType.DepthVision:
                        if (SaveData.Instance.UpgradeData.DepthVision >= upgrade.levels)
                        {
                            text.text = upgrade.upgradeName + " Purchased";
                            newButton.interactable = false;
                        }
                        break;
                    case Upgrade.UpgradeType.SwimSpeed:
                        if (SaveData.Instance.UpgradeData.SwimSpeed >= upgrade.levels)
                        {
                            text.text = upgrade.upgradeName + " Purchased";
                            newButton.interactable = false;
                        }
                        break;
                    case Upgrade.UpgradeType.Scanner:
                        if (SaveData.Instance.UpgradeData.ScannerUpgrade)
                        {
                            text.text = upgrade.upgradeName + " Purchased";
                            newButton.interactable = false;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            buttonTemplate.gameObject.SetActive(false);
        }

        private void HandleClick(Button button, Upgrade upgrade)
        {
            if (SaveData.Instance.UpgradeData.ResearchCurrency < upgrade.cost)
            {
                actionText.text = "Need More Research Points!";
                return;
            }

            SaveData.Instance.UpgradeData.RemoveCurrency(upgrade.cost);
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            switch (upgrade.upgradeType)
            {
                case Upgrade.UpgradeType.DepthVision:
                    SaveData.Instance.UpgradeData.DepthVision++;
                    if (SaveData.Instance.UpgradeData.DepthVision >= upgrade.levels)
                    {
                        text.text = upgrade.upgradeName + " Purchased";
                        button.interactable = false;
                    }
                    break;
                case Upgrade.UpgradeType.SwimSpeed:
                    SaveData.Instance.UpgradeData.SwimSpeed++;
                    if (SaveData.Instance.UpgradeData.SwimSpeed >= upgrade.levels)
                    {
                        text.text = upgrade.upgradeName + " Purchased";
                        button.interactable = false;
                    }
                    break;
                case Upgrade.UpgradeType.Scanner:
                    SaveData.Instance.UpgradeData.ScannerUpgrade = true;
                    button.interactable = false;
                    text.text = upgrade.upgradeName + " Purchased";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            actionText.text = "Purchased " + upgrade.upgradeName;
        }

        public void Activate()
        {
            canvasGroup.alpha = 1f;
            Time.timeScale = 0f;
            GameCore.Instance.PlayerBusy = true;
            isActive = true;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void Deactivate()
        {
            canvasGroup.alpha = 0f;
            Time.timeScale = 1f;
            GameCore.Instance.PlayerBusy = false;
            isActive = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            OnDeactivate?.Invoke();
        }
    }

    [Serializable]
    public class Upgrade
    {
        public enum UpgradeType
        {
            DepthVision,
            SwimSpeed,
            Scanner
        }

        public string upgradeName;
        public UpgradeType upgradeType;
        public int levels;
        public int cost;
    }
}
