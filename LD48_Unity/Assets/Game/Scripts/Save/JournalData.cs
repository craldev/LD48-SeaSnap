using System;
using System.Collections.Generic;
using System.Linq;
using LD48.Core;
using LD48.Gameplay.Entity;
using LD48.Utils;
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
			foreach (var entry in journalEntries)
			{
				entry.Initialize();
				entry.entity.Discover();
			}

			journalDictionary = journalEntries.ToDictionary(entity => entity.entity);
		}

		public void Add(Entity entity, JournalEntry journalEntry)
		{
			entity.Discover();

			journalEntries.Add(journalEntry);
			journalDictionary.Add(entity, journalEntry);
		}

		[Serializable]
		public class JournalEntry
		{
			public Entity.EntityType entityType;
			public string entityGUID;
			public string pictureFilePath;

			public Texture2D picture;
			public Entity entity;

			public JournalEntry(Entity entity, string pictureFilePath)
			{
				this.pictureFilePath = pictureFilePath;
				picture = TextureUtils.LoadTexture(pictureFilePath);
				entityType = entity.GetType();
				entityGUID = entity.GUID;
				this.entity = entity;
			}

			public void Initialize()
			{
				entity = GameCore.Instance.EntityLibrary.GetEntity(entityType, entityGUID);
				picture = TextureUtils.LoadTexture(pictureFilePath);
			}
		}
	}
}