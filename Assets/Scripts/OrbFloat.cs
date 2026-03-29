using UnityEngine;

public class OrbFloat : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatHeight = 0.2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}