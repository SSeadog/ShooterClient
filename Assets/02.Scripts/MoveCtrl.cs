using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour
{
    private Transform tr;
    private CharacterController controller;

    private float h = 0.0f;
    private float v = 0.0f;

    private float movSpeed = 5.0f;
    private float rotSpeed = 50.0f;

    private Vector3 movDir = Vector3.zero;

    void Start()
    {
        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        tr.Rotate(Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime * Vector3.up);

        movDir = (tr.forward * v) + (tr.right * h);
        movDir.y -= 20f * Time.deltaTime;

        controller.Move(movDir * movSpeed * Time.deltaTime);
    }
}
