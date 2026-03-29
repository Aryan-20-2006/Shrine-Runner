using UnityEngine;

public class GhostMode : MonoBehaviour
{
    public bool isGhost = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ActivateGhost()
    {
        if (isGhost) return;

        isGhost = true;
        Debug.Log("Ghost Mode Activated");

        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }
}