using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private bool alwaysRespawnAtSceneStart = true;
    [SerializeField] private Transform spawnPoint;

    private Vector3 sceneStartPosition;
    private bool hasSceneStartPosition;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            sceneStartPosition = player.transform.position;
            hasSceneStartPosition = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player died");

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            Transform playerTransform = collision.transform;

            // If player was parented to a moving platform, detach before teleporting.
            if (playerTransform.parent != null)
            {
                playerTransform.SetParent(null, true);
            }

            Vector3 respawnPosition;

            if (alwaysRespawnAtSceneStart && hasSceneStartPosition)
            {
                respawnPosition = sceneStartPosition;
            }
            else if (spawnPoint != null)
            {
                respawnPosition = spawnPoint.position;
            }
            else if (hasSceneStartPosition)
            {
                respawnPosition = sceneStartPosition;
            }
            else
            {
                respawnPosition = playerTransform.position;
            }

            playerTransform.position = respawnPosition;

            CameraFollow cameraFollow = Camera.main != null
                ? Camera.main.GetComponent<CameraFollow>()
                : null;

            if (cameraFollow != null)
            {
                cameraFollow.SnapToTarget();
            }
        }
    }
}