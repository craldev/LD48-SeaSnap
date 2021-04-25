using UnityEngine;
using CharacterController = LD48.Gameplay.Player.CharacterController;

namespace LD48.Gameplay.Util
{
    public class LightDistance : MonoBehaviour
    {
        [SerializeField]
        private Light light;

        [SerializeField]
        private Vector2 lightValue;

        [SerializeField]
        private Vector2 distanceLerp;

        private void Update()
        {
            var distance = (transform.position - CharacterController.Instance.transform.position).sqrMagnitude;
            var lerp = Mathf.InverseLerp(distanceLerp.x, distanceLerp.y, distance);
            light.intensity = Mathf.Lerp(lightValue.x, lightValue.y, lerp);
        }
    }
}
