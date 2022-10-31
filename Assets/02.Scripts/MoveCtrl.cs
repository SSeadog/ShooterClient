using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour
{
    protected NetworkManager _network;

    private Transform tr;
    private CharacterController controller;

    private float h = 0.0f;
    private float v = 0.0f;

    private float movSpeed = 5.0f;
    private float rotSpeed = 50.0f;

    private Vector3 movDir = Vector3.zero;

    void Start()
    {
        //StartCoroutine("CoSendPacket");
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 플레이어의 이동은 서버에서 돌아온 패킷값을 기반으로 움직이도록? 하는 게 좋다고 했던 거 같은데 이 부분을 어떻게 할지 생각해보기
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        tr.Rotate(Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime * Vector3.up);

        movDir = ((tr.forward * v) + (tr.right * h)).normalized;
    }

    IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            tr.Rotate(Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime * Vector3.up);

            movDir = ((tr.forward * v) + (tr.right * h)).normalized;
            //movDir.y -= 20f * Time.deltaTime;

            //controller.Move(movDir * movSpeed * Time.deltaTime);

            Vector3 movPos = transform.position + movDir * movSpeed;

            C_Move movePacket = new C_Move();
            movePacket.posX = movPos.x;
            movePacket.posY = movPos.y;
            movePacket.posZ = movPos.z;

            Vector3 rot = transform.rotation.eulerAngles;
            movePacket.rotY = rot.y;

            _network.Send(movePacket.Write());
        }
    }
}
