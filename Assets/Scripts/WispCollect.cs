using UnityEngine;

public class WispCollect : MonoBehaviour
{
    public LightManager manager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Wisp Collected");

            if (manager != null)
            {
                manager.CollectLight();
            }

            Destroy(gameObject);
        }
    }
}