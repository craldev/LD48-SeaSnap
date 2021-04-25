using System;
using System.Collections.Generic;
using System.Linq;
using LD48.Gameplay.Entity;
using UnityEngine;

namespace LD48.Core
{
	[CreateAssetMenu(fileName = "EntityLibrary", menuName = "SeaSnap/EntityLibrary", order = 1)]
	public class EntityLibrary : ScriptableObject
	{
		[SerializeField]
		private List<Entity> fishEntities;

		[SerializeField]
		private List<Entity> decoEntities;

		[SerializeField]
		private List<Entity> artifactEntities;

		public Dictionary<string, Entity> fishDictionary = new Dictionary<string, Entity>();
		public Dictionary<string, Entity> decoDictionary = new Dictionary<string, Entity>();
		public Dictionary<string, Entity> artifactDictionary = new Dictionary<string, Entity>();

		public void Initialize()
		{
			foreach (var fishEntity in fishEntities)
			{
				fishEntity.Initialize(Entity.EntityType.Fish);
			}

			foreach (var decoEntity in decoEntities)
			{
				decoEntity.Initialize(Entity.EntityType.Deco);
			}

			foreach (var artifactEntity in artifactEntities)
			{
				artifactEntity.Initialize(Entity.EntityType.Deco);
			}

			fishDictionary = fishEntities.ToDictionary(entity => entity.GUID);
			decoDictionary = decoEntities.ToDictionary(entity => entity.GUID);
			artifactDictionary = artifactEntities.ToDictionary(entity => entity.GUID);

		}

		public Entity GetEntity(Entity.EntityType entityType, string entityGuid)
		{
			return entityType switch
			{
				Entity.EntityType.Fish => fishDictionary[entityGuid],
				Entity.EntityType.Deco => decoDictionary[entityGuid],
				Entity.EntityType.Artifact => artifactDictionary[entityGuid],
				_ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
			};
		}
	}
}