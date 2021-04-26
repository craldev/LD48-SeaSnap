using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD48.Gameplay.UI
{
	public class UpgradeShopActivator : MonoBehaviour
	{
		[SerializeField]
		private UpgradeShop upgradeShop;

		[SerializeField]
		private GameObject interactText;

		private InputAction activateAction;

		private void Start()
		{
			var map = new InputActionMap("Upgrade Shop");
			activateAction = map.AddAction("activate", binding: "<Keyboard>/e");

			activateAction.Enable();

			upgradeShop.OnDeactivate += HandleDeactivate;
		}

		public void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				activateAction.performed += HandleActivate;
				interactText.SetActive(true);
			}
		}

		public void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				activateAction.performed -= HandleActivate;
				interactText.SetActive(false);
			}
		}

		private void HandleActivate(InputAction.CallbackContext context)
		{
			upgradeShop.Activate();
			activateAction.performed -= HandleActivate;
			interactText.SetActive(false);
		}

		private void HandleDeactivate()
		{
			activateAction.performed += HandleActivate;
			interactText.SetActive(true);
		}
	}
}