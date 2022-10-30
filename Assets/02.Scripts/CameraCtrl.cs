using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject target;

    float distance = 10f;
    float height = 3f;

    Vector3 offset;

    void Start()
    {
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, Time.deltaTime * 10.0f);
        Quaternion rot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        transform.position = target.transform.position - (rot * Vector3.forward * distance) + Vector3.up * height;
    }
}
