using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
    }
}
