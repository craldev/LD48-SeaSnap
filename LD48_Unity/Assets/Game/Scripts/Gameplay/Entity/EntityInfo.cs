using UnityEngine;

namespace LD48.Gameplay.Entity
{
    public class EntityInfo : MonoBehaviour
    {
        [SerializeField]
        private string name = "DEFAULT_NAME";
        public string Name => name;
    }
}
