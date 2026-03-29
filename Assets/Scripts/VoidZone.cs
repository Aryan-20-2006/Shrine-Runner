using UnityEngine;

public class VoidZone : MonoBehaviour
{
    [SerializeField] private bool makePlayerPhaseThroughThisVoid = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandlePlayerContact(collision.transform);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandlePlayerContact(collision.transform);
    }

    void HandlePlayerContact(Transform other)
    {
        if (other == null || !other.CompareTag("Player"))
            return;

        GhostMode ghost = other.GetComponent<GhostMode>();

        if (ghost != null)
            ghost.ActivateGhost();

        if (makePlayerPhaseThroughThisVoid)
            IgnoreVoidCollisions(other);
    }

    void IgnoreVoidCollisions(Transform player)
    {
        Collider2D[] playerColliders = player.GetComponentsInChildren<Collider2D>();
        Collider2D[] voidColliders = GetComponentsInChildren<Collider2D>();

        foreach (Collider2D playerCollider in playerColliders)
        {
            if (playerCollider == null) continue;

            foreach (Collider2D voidCollider in voidColliders)
            {
                if (voidCollider == null) continue;
                Physics2D.IgnoreCollision(playerCollider, voidCollider, true);
            }
        }
    }
}