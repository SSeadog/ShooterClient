using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    private float lifeTime = 5.0f;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 10.0f;
        Destroy(gameObject, lifeTime);
    }
}