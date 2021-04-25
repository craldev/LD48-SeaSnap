using System;
using System.Collections.Generic;
using System.Linq;
using LD48.Core;
using LD48.Gameplay.Entity;
using UnityEngine;

namespace LD48.Save
{
	[Serializable]
	public class JournalData
	{
		[SerializeField]
		private List<JournalEntry> journalEntries = new List<JournalEntry>();

		public Dictionary<Entity, JournalEntry> journalDictionary = new Dictionary<Entity, JournalEntry>();

		public void Initialize()
		{
			journalDictionary = journalEntries.ToDictionary(entity => entity.entity);
		}

		public void Add(Entity entity, JournalEntry journalEntry)
		{
			journalEntries.Add(journalEntry);
			journalDictionary.Add(entity, journalEntry);
		}

		[Serializable]
		public class JournalEntry
		{
			public Entity.EntityType entityType;
			public string entityGUID;
			public string pictureFilePath;

			public Entity entity;

			public JournalEntry(Entity entity, string pictureFilePath)
			{
				this.pictureFilePath = pictureFilePath;
				entityType = entity.GetType();
				entityGUID = entity.GUID;
				this.entity = entity;
			}

			public void Initialize()
			{
				entity = GameCore.Instance.EntityLibrary.GetEntity(entityType, entityGUID);
			}
		}
	}
}