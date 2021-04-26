using UnityEngine;
using Random = UnityEngine.Random;

namespace LD48.Gameplay.Fish
{
    public class WanderMovement : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;

        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField]
        private float swimRange = 3f;

        [SerializeField]
        private Vector2 swimPowerRange = new Vector2(0.5f, 1f);

        [SerializeField]
        private float avoidanceDistance = 1f;

        [SerializeField]
        private Vector2 updatePosTimerRange = new Vector2(1f, 4f);

        [SerializeField]
        private float stoppingDistance = 0.2f;

        [SerializeField]
        private Vector3 movementBias = new Vector3(1f, 1f, 1f);

        private Vector3 startingPos;
        private Vector3 targetPos;

        private float swimPower;
        private float timer;
        private float lastFindTime;
        private UnityEngine.Camera camera;

        private void Start()
        {
            camera = UnityEngine.Camera.main;

            startingPos = transform.position;
            FindMovementPosition();
        }

        private void Update()
        {
            Wander();
        }

        private void Wander()
        {
            if (lastFindTime < Time.time - timer)
            {
                FindMovementPosition();
            }

            var vector = targetPos - transform.position;
            if (vector.magnitude > stoppingDistance)
            {
                rigidbody.AddForce(vector.normalized * (swimPower * Random.Range(0.5f, 1.5f)) * Time.deltaTime, ForceMode.VelocityChange);
            }

            sprite.transform.localScale = new Vector3(camera.transform.InverseTransformDirection(rigidbody.velocity).x < 0 ? -1f : 1f, 1f, 1f);
        }

        private void FindMovementPosition()
        {
            swimPower = Random.Range(swimPowerRange.x, swimPowerRange.y);

            var randomModifier = Random.insideUnitSphere * swimRange;
            targetPos = startingPos + Vector3.Scale(randomModifier, movementBias);
            lastFindTime = Time.time;
            timer = Random.Range(updatePosTimerRange.x, updatePosTimerRange.y);

            var vector = targetPos - transform.position;
            var ray = new Ray(transform.position, vector.normalized);
            if (Physics.Raycast(ray, out var hit, vector.magnitude, LayerMask.NameToLayer("Default")))
            {
                targetPos = hit.point - vector.normalized * avoidanceDistance;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(targetPos, 0.5f);
        }
    }
}
