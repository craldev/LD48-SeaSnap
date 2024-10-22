﻿using UnityEngine;

namespace LD48.Gameplay.Entity
{
	[CreateAssetMenu(fileName = "Entity", menuName = "SeaSnap/Entity", order = 1)]
	public class Entity : ScriptableObject
	{
		public enum EntityType
		{
			Creature,
			Misc,
			Artifact
		}

		[SerializeField]
		private string guid = System.Guid.NewGuid().ToString();
		public string GUID { get => guid; set => guid = value; }

		[SerializeField]
		private string entityName = "DEFAULT_NAME";
		public string DisplayName => isDiscovered ? entityName : "???";
		public string EntityName => entityName;

		[SerializeField]
		private string description = "DESCRIPTION";
		public string Description => description;

		private EntityType entityType;
		public EntityType Type => entityType;

		private bool isDiscovered;
		public bool IsDiscovered => isDiscovered;

		public void Initialize(EntityType entityType)
		{
			isDiscovered = false;
			this.entityType = entityType;
		}

		public EntityType GetType()
		{
			return entityType;
		}

		public void Discover()
		{
			isDiscovered = true;
		}
	}
}