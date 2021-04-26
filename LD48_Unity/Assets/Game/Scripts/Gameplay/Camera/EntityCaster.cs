using LD48.Gameplay.Entity;
using LD48.Save;
using TMPro;
using UnityEngine;

namespace LD48.Gameplay.Camera
{
    public class EntityCaster : MonoBehaviour
    {
        public static EntityCaster Instance { get; private set; }
        public static Entity.Entity CurrentActiveEntity { get; private set; }

        [SerializeField]
        private UnityEngine.Camera camera;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private TextMeshProUGUI textMeshPro;

        [SerializeField]
        private float defaultDistance = 7f;

        private bool force;
        private void Start()
        {
            Instance = this;
        }

        private void Update()
        {
            var distance = defaultDistance * Mathf.InverseLerp(2.5f, 0f, RenderSettings.fogDensity);
            var ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            if (Physics.Raycast(ray, out var hit, 20f, layerMask))
            {
                if (hit.transform.TryGetComponent(out EntityInfo entityInfo) || hit.transform.parent.TryGetComponent(out entityInfo))
                {
                    if (entityInfo.Entity != null)
                    {
                        if (hit.distance > distance)
                        {
                            if (SaveData.Instance.UpgradeData.ScannerUpgrade && PictureTaker.IsActive || force)
                            {
                                textMeshPro.text = "Too far, try getting closer!";
                                textMeshPro.transform.parent.gameObject.SetActive(true);
                            }

                            return;
                        }
                        CurrentActiveEntity = entityInfo.Entity;
                        if (SaveData.Instance.UpgradeData.ScannerUpgrade && PictureTaker.IsActive || force)
                        {
                            textMeshPro.text = entityInfo.Entity.DisplayName;
                            textMeshPro.transform.parent.gameObject.SetActive(true);
                        }
                        else
                        {
                            textMeshPro.text = "";
                            textMeshPro.transform.parent.gameObject.SetActive(false);
                        }
                        return;
                    }
                }
            }

            CurrentActiveEntity = null;
            textMeshPro.text = "";
            textMeshPro.transform.parent.gameObject.SetActive(false);
        }

        public void ForceDisplayName(bool value)
        {
            force = value;
        }
    }
}
