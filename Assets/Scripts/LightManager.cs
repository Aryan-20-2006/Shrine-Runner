using UnityEngine;

public class LightManager : MonoBehaviour
{
    public int lightCollected = 0;
    

    public void CollectLight()
    {
        lightCollected++;

        Debug.Log("Light Collected: " + lightCollected);
    }
}