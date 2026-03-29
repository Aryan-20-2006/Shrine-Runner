using UnityEngine;

public class ParticleMoveToShrine:MonoBehaviour
{
    public Transform target;
    public float speed=3f;


    void Update()
    {
        if(target==null) return;

        transform.position=Vector3.MoveTowards(
            transform.position,
            target.position,
            speed *Time.deltaTime
        );
    }
}