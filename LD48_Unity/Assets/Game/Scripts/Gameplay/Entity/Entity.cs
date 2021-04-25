using UnityEngine;

namespace LD48.Gameplay.Entity
{
	[CreateAssetMenu(fileName = "Entity", menuName = "SeaSnap/Entity", order = 1)]
	public class Entity : ScriptableObject
	{
		public enum EntityType
		{
			Fish,
			Deco,
			Artifact
		}

		[SerializeField]
		private string guid = System.Guid.NewGuid().ToString();
		public string GUID => guid;

		[SerializeField]
		private string entityName = "DEFAULT_NAME";
		public string EntityName => entityName;

		private EntityType entityType;

		public void Initialize(EntityType entityType)
		{
			this.entityType = entityType;
		}

		public EntityType GetType()
		{
			return entityType;
		}
	}
}