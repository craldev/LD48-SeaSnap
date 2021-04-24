using UnityEngine;

namespace LD48.Gameplay.Util
{
    public class PhysicsManager : MonoBehaviour
    {
        private void Start()
        {
            Physics.autoSimulation = false;
        }

        private void Update()
        {
            Physics.Simulate(Time.deltaTime);
        }
    }
}
