using UnityEngine;

public class OrbPulse : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.1f;

    Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * scale;
        transform.Rotate(0,0,30*Time.deltaTime);
    }
}