using UnityEngine;

public class LightWave : MonoBehaviour
{
    public float expandSpeed = 6f;
    public float maxScale = 20f;

    SpriteRenderer sr;
    bool expanding = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!expanding) return;

        // expand outward
        transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;

        // fade out gradually
        Color c = sr.color;
        c.a -= Time.deltaTime * 0.4f;
        sr.color = c;

        if (transform.localScale.x >= maxScale)
        {
            expanding = false;
            gameObject.SetActive(false);
        }
    }

    public void TriggerWave()
    {
        gameObject.SetActive(true);

        // reset wave size
        transform.localScale = Vector3.one * 0.1f;

        // reset color
        sr.color = new Color(1f, 0.95f, 0.6f, 0.4f);

        expanding = true;
    }
}