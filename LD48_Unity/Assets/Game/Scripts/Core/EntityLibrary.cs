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
		private List<Entity> fishEntities = new List<Entity>();

		[SerializeField]
		private List<Entity> decoEntities = new List<Entity>();

		[SerializeField]
		private List<Entity> artifactEntities = new List<Entity>();

		public Dictionary<string, Entity> fishDictionary = new Dictionary<string, Entity>();
		public Dictionary<string, Entity> decoDictionary = new Dictionary<string, Entity>();
		public Dictionary<string, Entity> artifactDictionary = new Dictionary<string, Entity>();

		private void OnValidate()
		{
			fishEntities.Clear();
			var allFish = Resources.LoadAll<EntityInfo>("Entities/Fish");
			fishEntities.AddRange(allFish.Where(fish=> fish.Entity != null).Select(fish => fish.Entity));

			decoEntities.Clear();
			var allDeco = Resources.LoadAll<EntityInfo>("Entities/Decoration");
			decoEntities.AddRange(allDeco.Where(deco=> deco.Entity != null).Select(deco => deco.Entity));

			artifactEntities.Clear();
			var allArtifacts = Resources.LoadAll<EntityInfo>("Entities/Artifacts");
			artifactEntities.AddRange(allArtifacts.Where(artifact=> artifact.Entity != null).Select(artifact => artifact.Entity));
		}

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