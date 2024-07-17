using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float speed = 20f;
    private float lifetime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        // as soon as this gameobject is spawned, it goes forward with this velocity
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(this.gameObject, lifetime);
    }

    
}
