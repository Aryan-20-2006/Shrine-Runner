using UnityEngine;

public class PlantSway : MonoBehaviour
{
    public float swayAmount = 2f;
    public float swaySpeed = 0.9f;

    public Transform player;
    public float reactDistance = 1.5f;
    public float reactStrength = 3f;

    Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;

        // random variation so plants aren't synchronized
        swaySpeed += Random.Range(-0.3f, 0.3f);
        swayAmount += Random.Range(-0.5f, 0.5f);
    }

    void Update()
    {
        float windAngle = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        float playerInfluence = 0f;

        if (player != null)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance < reactDistance)
            {
                float direction = transform.position.x - player.position.x;
                playerInfluence = direction * reactStrength;
            }
        }

        float finalAngle = windAngle + playerInfluence;

        transform.rotation = startRotation * Quaternion.Euler(0, 0, finalAngle);
    }
}