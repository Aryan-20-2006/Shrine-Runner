using UnityEngine;

public class SpiritBarrier : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        GhostMode ghost = collision.gameObject.GetComponent<GhostMode>();

        if(ghost != null && ghost.isGhost)
        {
            Physics2D.IgnoreCollision(
                collision.collider,
                GetComponent<Collider2D>()
            );
        }
    }
}