using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.15f;

    float shakeTimer = 0f;

    public Vector3 GetShakeOffset()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            return new Vector3(x, y, 0f);
        }

        return Vector3.zero;
    }

    public void Shake()
    {
        shakeTimer = shakeDuration;
    }
}