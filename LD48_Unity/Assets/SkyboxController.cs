using System;
using System.Collections.Generic;
using LD48.Save;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private List<DepthValue> depthValues = new List<DepthValue>();

    [SerializeField]
    private int currentIndex = 0;

    [SerializeField]
    private float currentDepth;

    [SerializeField]
    private Vector2 playerDepthRange;

    private void Start()
    {
        playerDepthRange.x = depthValues[0].depth;
        playerDepthRange.y = depthValues[depthValues.Count - 1].depth;
    }

    private void Update()
    {
        var nextIndex = Mathf.Clamp(currentIndex + 1, 0, depthValues.Count - 1);

        if (depthValues[currentIndex].depth < player.transform.position.y)
        {
            currentIndex--;
        }
        else if (depthValues[nextIndex].depth > player.transform.position.y)
        {
            currentIndex++;
        }
        currentIndex = Mathf.Clamp(currentIndex, 0, depthValues.Count - 1);

        currentDepth = Mathf.InverseLerp(depthValues[currentIndex].depth, depthValues[nextIndex].depth, player.transform.position.y);

        var cameraBackgroundColor = Color.Lerp(depthValues[currentIndex].color, depthValues[nextIndex].color, currentDepth % 1);
        camera.backgroundColor = cameraBackgroundColor;
        RenderSettings.fogColor = cameraBackgroundColor;

        RenderSettings.fogDensity = Mathf.Lerp(depthValues[currentIndex].GetDensity(), depthValues[nextIndex].GetDensity(), currentDepth % 1);
    }
}

[Serializable]
public struct DepthValue
{
    public Color color;
    public float fogDensity;
    public float depth;

    public float GetDensity() => fogDensity * visionRange[SaveData.Instance.UpgradeData.DepthVision];

    [SerializeField]
    private float[] visionRange;
}
