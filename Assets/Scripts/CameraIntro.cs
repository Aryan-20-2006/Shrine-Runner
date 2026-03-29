using UnityEngine;
using System.Collections;

public class CameraIntro : MonoBehaviour
{
    public Transform shrine;
    public Transform player;
    [SerializeField] private bool playIntroSequence = false;

    public float panTime = 2f;
    public float waitTime = 1.5f;
    [Header("Pan Easing")]
    public AnimationCurve panEase = new AnimationCurve(
        new Keyframe(0f, 0f, 0f, 0.8f),
        new Keyframe(0.7f, 0.9f, 1.2f, 0.4f),
        new Keyframe(1f, 1f, 0f, 0f)
    );

    CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = GetComponent<CameraFollow>();

        if (player == null)
        {
            GameObject taggedPlayer = GameObject.FindGameObjectWithTag("Player");
            if (taggedPlayer != null)
                player = taggedPlayer.transform;
        }

        if (cameraFollow == null)
        {
            this.enabled = false;
            return;
        }

        if (cameraFollow.target == null && player != null)
            cameraFollow.target = player;

        // Default behavior: always track player from the very first frame.
        if (!playIntroSequence)
        {
            cameraFollow.enabled = true;
            cameraFollow.SnapToTarget();
            this.enabled = false;
            return;
        }

        cameraFollow.enabled = false;

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        if (shrine == null || player == null)
        {
            if (cameraFollow != null)
                cameraFollow.enabled = true;

            this.enabled = false;
            yield break;
        }

        // Always begin intro from the player's camera framing so the player is visible at start.
        transform.position = cameraFollow.GetCameraPositionForTarget(player, 0f);

        // move to shrine
        Vector3 startPos = transform.position;
        Vector3 shrinePos = cameraFollow != null
            ? cameraFollow.GetCameraPositionForTarget(shrine, 0f)
            : new Vector3(shrine.position.x, shrine.position.y, transform.position.z);

        float t = 0;

        while (t < panTime)
        {
            float normalized = Mathf.Clamp01(t / panTime);
            float eased = panEase != null ? panEase.Evaluate(normalized) : normalized;
            transform.position = Vector3.Lerp(startPos, shrinePos, eased);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = shrinePos;

        yield return new WaitForSeconds(waitTime);

        // move back to player
        Vector3 playerPos = cameraFollow != null
            ? cameraFollow.GetCameraPositionForTarget(player, 0f)
            : new Vector3(player.position.x, player.position.y, transform.position.z);

        t = 0;
        startPos = transform.position;

        while (t < panTime)
        {
            float normalized = Mathf.Clamp01(t / panTime);
            float eased = panEase != null ? panEase.Evaluate(normalized) : normalized;
            transform.position = Vector3.Lerp(startPos, playerPos, eased);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = playerPos;

        // enable camera follow again
        if (cameraFollow != null)
        {
            cameraFollow.SnapToTarget();
            cameraFollow.enabled = true;
        }

        this.enabled = false;
    }
}