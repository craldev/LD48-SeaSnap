using UnityEngine;

namespace LD48.Utils
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField]
        private Vector3 rotation;

        [SerializeField]
        private float speed;

        private void Update()
        {
            transform.Rotate(rotation * speed * Time.deltaTime, Space.Self);
        }
    }
}
