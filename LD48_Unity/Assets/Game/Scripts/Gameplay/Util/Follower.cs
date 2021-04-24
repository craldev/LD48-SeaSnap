using UnityEngine;

namespace LD48.Gameplay.Util
{
    public class Follower : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private float dampTime;

        private Vector3 followVelocity;

        private void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref followVelocity, dampTime * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, dampTime * Time.deltaTime);
        }
    }
}
