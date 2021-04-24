using System.Collections;
using System.Collections.Generic;
using LD48.Gameplay.Entity;
using TMPro;
using UnityEngine;

public class EntityCaster : MonoBehaviour
{
    public static EntityCaster Instance { get; private set; }

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    [SerializeField]
    private float defaultDistance = 7f;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        var distance = defaultDistance * Mathf.InverseLerp(0.5f, 0.1f, RenderSettings.fogDensity);
        Debug.Log(distance);
        var ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        if (Physics.Raycast(ray, out var hit, distance, layerMask))
        {
            if (hit.transform.TryGetComponent<EntityInfo>(out var entityInfo))
            {
                textMeshPro.text = entityInfo.Name;
                return;
            }
        }

        textMeshPro.text = "";
    }
}
