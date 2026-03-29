using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShrinePulse : MonoBehaviour
{
    public Light2D shrineLight;

    public float minIntensity = 0.6f;
    public float maxIntensity = 1.2f;
    public float pulseSpeed = 2f;

    void Update()
    {
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f;
        shrineLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
    }
}