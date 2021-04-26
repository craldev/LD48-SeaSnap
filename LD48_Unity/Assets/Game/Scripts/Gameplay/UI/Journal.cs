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
		public static Journal Instance { get; private set; }

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private JournalEntryPreview entryTemplate;

		[SerializeField]
		private Transform entryParent;

		[SerializeField]
		private TextMeshProUGUI searchType;

		[SerializeField]
		private TextMeshProUGUI entryName;

		[SerializeField]
		private TextMeshProUGUI entryDescription;

		[SerializeField]
		private RawImage entryPicture;

		[SerializeField]
		private TextMeshProUGUI collectedText;

		[SerializeField]
		private Button creatureButton;

		[SerializeField]
		private Button artifactButton;

		[SerializeField]
		private Button miscButton;

		private bool isActive;
		private InputAction activateAction;
		private Entity.Entity.EntityType lastEntityType = Entity.Entity.EntityType.Artifact;
		private Entity.Entity lastSelectedEntity;

		public void Start()
		{
			Instance = this;

			canvasGroup.alpha = 0f;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			entryTemplate.gameObject.SetActive(false);
			var map = new InputActionMap("Journal");
			activateAction = map.AddAction("activate", binding: "<Keyboard>/t");

			activateAction.performed += HandleActivate;
			activateAction.Enable();
			Display(null);
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

		public void Activate(Entity.Entity entity)
		{
			lastEntityType = entity.Type;
			lastSelectedEntity = entity;
			Activate();
		}

		public void Activate()
		{
			if (GameCore.Instance.PlayerBusy) return;

			canvasGroup.alpha = 1f;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;

			Time.timeScale = 0f;
			GameCore.Instance.PlayerBusy = true;
			isActive = true;

			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
			UpdateEntries(lastEntityType);
		}

		public void ChangeType(int index)
		{
			lastEntityType = (Entity.Entity.EntityType)index;
			UpdateEntries((Entity.Entity.EntityType)index);
		}

		private void UpdateEntries(Entity.Entity.EntityType entityType)
		{
			searchType.text = entityType.ToString();
			foreach (Transform child in entryParent)
			{
				Destroy(child.gameObject);
			}

			var allEntities = new List<Entity.Entity>();

			switch (entityType)
			{
				case Entity.Entity.EntityType.Fish:
					allEntities = GameCore.Instance.EntityLibrary.fishDictionary.Values.ToList();
					creatureButton.interactable = false;
					miscButton.interactable = true;
					artifactButton.interactable = true;
					break;
				case Entity.Entity.EntityType.Deco:
					allEntities = GameCore.Instance.EntityLibrary.decoDictionary.Values.ToList();
					creatureButton.interactable = true;
					miscButton.interactable = false;
					artifactButton.interactable = true;
					break;
				case Entity.Entity.EntityType.Artifact:
					allEntities = GameCore.Instance.EntityLibrary.artifactDictionary.Values.ToList();
					creatureButton.interactable = true;
					miscButton.interactable = true;
					artifactButton.interactable = false;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
			}

			allEntities = allEntities.OrderBy(c => !c.IsDiscovered).ThenBy(c=>c.name).ToList();

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

				if (lastSelectedEntity == entity)
				{
					Display(entry);
				}
				else if ((lastSelectedEntity != null && lastSelectedEntity.Type != lastEntityType || lastSelectedEntity == null) && entry.Entity.IsDiscovered)
				{
					Display(entry);
				}
			}

			collectedText.text = allEntities.Count(c => c.IsDiscovered) + " / " + allEntities.Count + " Collected";

			if (lastSelectedEntity == null || lastSelectedEntity.Type != lastEntityType)
			{
				Display(null);
			}
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
			if (entry == null)
			{
				entryPicture.texture = null;
				entryName.text = "";
				entryDescription.text = "";
				lastSelectedEntity = null;
			}
			else
			{
				entryPicture.texture = entry.Picture.texture;
				entryName.text = entry.Entity.DisplayName;
				entryDescription.text = entry.Entity.Description;
				lastSelectedEntity = entry.Entity;
			}
		}
	}
}