using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [SerializeField] private bool autoFindPlayerTarget = true;

    [Header("Follow Settings")]
    public float smoothTime = 0.2f;
    public Vector2 offset = new Vector2(0f, 1f);

    [Header("Look Ahead")]
    public float lookAheadDistance = 2f;
    public float lookAheadSmooth = 0.1f;

    [Header("Camera Bounds")]
    public BoxCollider2D bounds;

    Vector3 velocity = Vector3.zero;
    float currentLookAhead = 0f;
    bool snappedToTargetOnce;

    float minX, maxX, minY, maxY;
    bool hasValidBounds;

    void Start()
    {
        TryAssignTarget();
        CacheBounds();
        SnapToTarget();
    }

    void OnEnable()
    {
        TryAssignTarget();
        CacheBounds();
        SnapToTarget();
    }

    void CacheBounds()
    {
        hasValidBounds = false;

        if (bounds != null)
        {
            minX = bounds.bounds.min.x;
            maxX = bounds.bounds.max.x;
            minY = bounds.bounds.min.y;
            maxY = bounds.bounds.max.y;

            hasValidBounds = (maxX - minX) > 0.01f && (maxY - minY) > 0.01f;
        }
    }

    void TryAssignTarget()
    {
        if (target != null || !autoFindPlayerTarget)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            return;
        }

        PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
            target = playerMovement.transform;
    }

    void LateUpdate()
    {
        if (target == null)
            TryAssignTarget();

        if (target == null) return;

        if (!snappedToTargetOnce)
        {
            SnapToTarget();
        }

        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        float moveX = 0f;

        // detect movement direction
        if (rb != null)
            moveX = rb.linearVelocity.x;

        float targetLookAhead = Mathf.Abs(moveX) > 0.01f
            ? Mathf.Sign(moveX) * lookAheadDistance
            : 0f;

        currentLookAhead = Mathf.Lerp(
            currentLookAhead,
            targetLookAhead,
            lookAheadSmooth
        );

        Vector3 targetPos = GetCameraPositionForTarget(target, currentLookAhead);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }

    public Vector3 GetCameraPositionForTarget(Transform focusTarget, float lookAhead = 0f)
    {
        if (focusTarget == null)
            return transform.position;

        Vector3 targetPos = new Vector3(
            focusTarget.position.x + lookAhead + offset.x,
            focusTarget.position.y + offset.y,
            transform.position.z
        );

        // Clamp only when bounds are valid.
        if (hasValidBounds)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        }

        return targetPos;
    }

    public void SnapToTarget()
    {
        if (target == null) return;

        velocity = Vector3.zero;
        currentLookAhead = 0f;
        transform.position = GetCameraPositionForTarget(target, 0f);
        snappedToTargetOnce = true;
    }
}