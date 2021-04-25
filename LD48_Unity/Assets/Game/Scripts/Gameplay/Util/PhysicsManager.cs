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
            if (Time.timeScale <= 0) return;
            Physics.Simulate(Time.deltaTime);
        }
    }
}
