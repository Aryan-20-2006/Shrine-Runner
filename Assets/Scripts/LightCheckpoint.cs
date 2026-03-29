using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCheckpoint : MonoBehaviour
{
    public LightManager manager;
    public Light2D globalLight;

    public ParticleSystem shrineParticles;
    public ParticleSystem fireflies;

    public CameraShake cameraShake;
    public LightWave lightWave;

    public int requiredLight = 5;

    public float targetIntensity = 1.5f;
    public float lightSpeed = 1f;

    bool activated = false;

    void Update()
    {
        if (activated && globalLight != null)
        {
            globalLight.intensity = Mathf.Lerp(
                globalLight.intensity,
                targetIntensity,
                Time.deltaTime * lightSpeed
            );
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (activated) return;
        if (manager == null) return;

        if (manager.lightCollected >= requiredLight)
        {
            ActivateShrine();
        }
    }

    void ActivateShrine()
    {
        activated = true;

        Debug.Log("Shrine Activated!");

        if (shrineParticles != null)
            shrineParticles.Play();

        if (fireflies != null)
            fireflies.Play();

        if (cameraShake != null)
            cameraShake.Shake();

        if (lightWave != null)
            lightWave.TriggerWave();

        ClearRemainingWisps();
    }

    void ClearRemainingWisps()
    {
        WispCollect[] wisps = FindObjectsByType<WispCollect>(FindObjectsSortMode.None);

        foreach (WispCollect wisp in wisps)
        {
            Destroy(wisp.gameObject);
        }
    }
}