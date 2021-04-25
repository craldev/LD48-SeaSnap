using UnityEngine;

namespace LD48.Gameplay.Entity
{
    public class EntityInfo : MonoBehaviour
    {
        [SerializeField]
        private Entity entity;
        public Entity Entity => entity;
    }
}
