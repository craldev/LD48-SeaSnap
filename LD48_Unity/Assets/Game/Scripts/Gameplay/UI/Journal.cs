using System;
using System.Collections.Generic;
using System.Linq;
using LD48.Core;
using LD48.Save;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LD48.Gameplay.UI
{
	public class Journal : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private JournalEntryPreview entryTemplate;

		[SerializeField]
		private Transform entryParent;

		[SerializeField]
		private TextMeshProUGUI entryName;

		[SerializeField]
		private TextMeshProUGUI entryDescription;

		[SerializeField]
		private RawImage entryPicture;

		[SerializeField]
		private TextMeshProUGUI collectedText;

		private bool isActive;
		private InputAction activateAction;

		public void Start()
		{
			entryTemplate.gameObject.SetActive(false);
			var map = new InputActionMap("Journal");
			activateAction = map.AddAction("activate", binding: "<Keyboard>/t");

			activateAction.performed += HandleActivate;
			activateAction.Enable();
		}

		private void HandleActivate(InputAction.CallbackContext context)
		{
			if (isActive)
			{
				Deactivate();
			}
			else
			{
				Activate();
			}
		}

		public void Activate()
		{
			canvasGroup.alpha = 1f;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;

			Time.timeScale = 0f;
			GameCore.Instance.PlayerBusy = true;
			isActive = true;

			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
			UpdateEntries(Entity.Entity.EntityType.Artifact);
		}

		private void UpdateEntries(Entity.Entity.EntityType entityType)
		{
			foreach (Transform child in entryParent)
			{
				Destroy(child.gameObject);
			}

			var allEntities = new List<Entity.Entity>();

			switch (entityType)
			{
				case Entity.Entity.EntityType.Fish:
					allEntities = GameCore.Instance.EntityLibrary.fishDictionary.Values.ToList();
					break;
				case Entity.Entity.EntityType.Deco:
					allEntities = GameCore.Instance.EntityLibrary.decoDictionary.Values.ToList();
					break;
				case Entity.Entity.EntityType.Artifact:
					allEntities = GameCore.Instance.EntityLibrary.artifactDictionary.Values.ToList();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
			}

			allEntities = allEntities.OrderBy(c => c.name).ThenBy(c=>c.IsDiscovered).ToList();

			foreach (var entity in allEntities)
			{
				var entry = Instantiate(entryTemplate, entryParent);
				entry.Entity = entity;
				if (SaveData.Instance.JournalData.journalDictionary.ContainsKey(entity))
				{
					var journalEntry = SaveData.Instance.JournalData.journalDictionary[entity];
					entry.Picture.texture = journalEntry.picture;
				}
				entry.Text.text = entity.DisplayName;

				if (!entity.IsDiscovered)
				{
					entry.GetComponent<Button>().interactable = false;
				}
				entry.gameObject.SetActive(true);
			}

			collectedText.text = allEntities.Count(c => c.IsDiscovered) + " / " + allEntities.Count + " Collected";
		}

		public void Deactivate()
		{
			canvasGroup.alpha = 0f;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			Time.timeScale = 1f;
			GameCore.Instance.PlayerBusy = false;
			isActive = false;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		public void Display(JournalEntryPreview entry)
		{
			entryPicture.texture = entry.Picture.texture;
			entryName.text = entry.Entity.DisplayName;
			entryDescription.text = entry.Entity.Description;

		}
	}
}