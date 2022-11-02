using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayer : Player
{
    private float h = 0.0f;
    private float v = 0.0f;

    private float movSpeed = 5.0f;
    private float rotSpeed = 50.0f;

    private Vector3 movDir = Vector3.zero;

    void Start()
    {
        StartCoroutine("CoSendPacket");
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        firePos = transform.Find("firePos");
        bulletPrefab = (GameObject)Resources.Load("Bullet");

        RectTransform[] rects = transform.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform rect in rects)
        {
            if (rect.name == "Hp")
            {
                Hpbar = rect;
            }
        }

        controller = gameObject.GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animation>();

        AnimationClip _idleAnimClip = anim.GetClip("idle");
        AnimationClip _runForwardAnimClip = anim.GetClip("runForward");
        AnimationClip _runBackwardAnimClip = anim.GetClip("runBackward");
        AnimationClip _runRightAnimClip = anim.GetClip("runRight");
        AnimationClip _runLeftAnimClip = anim.GetClip("runLeft");

        animClips = new AnimationClip[5];
        animClips[0] = _idleAnimClip;
        animClips[1] = _runForwardAnimClip;
        animClips[2] = _runBackwardAnimClip;
        animClips[3] = _runRightAnimClip;
        animClips[4] = _runLeftAnimClip;
    }

    void Update()
    {
        if (isDie)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        movDir = ((transform.forward * v) + (transform.right * h)).normalized;
        controller.Move(movDir * movSpeed * Time.deltaTime);
        transform.Rotate(Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime * Vector3.up);

        Vector3 localVelocity = transform.InverseTransformDirection(controller.velocity);
        Vector3 forwardDir = new Vector3(0f, 0f, localVelocity.z);
        Vector3 rightDir = new Vector3(localVelocity.x, 0f, 0f);

        if (forwardDir.z >= 0.1f)
        {
            animState = AnimState.runForward;
        }
        else if (forwardDir.z <= -0.1f)
        {
            animState = AnimState.runBackward;
        }
        else if (rightDir.x >= 0.1f)
        {
            animState = AnimState.runRight;
        }
        else if (rightDir.x <= -0.1f)
        {
            animState = AnimState.runLeft;
        }
        else
        {
            animState = AnimState.idle;
        }

        anim.CrossFade(animClips[(int)animState].name, 0.2f);
    }

    IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            if (isDie)
                continue;

            C_Move movePacket = new C_Move();
            movePacket.posX = transform.position.x;
            movePacket.posY = transform.position.y;
            movePacket.posZ = transform.position.z;

            Vector3 velocity = controller.velocity;
            movePacket.velX = velocity.x;
            movePacket.velY = velocity.y;
            movePacket.velZ = velocity.z;

            Vector3 rot = transform.rotation.eulerAngles;
            movePacket.rotY = rot.y;

            _network.Send(movePacket.Write());

            C_Animate animPacket = new C_Animate();
            animPacket.animIndex = (int)animState;

            _network.Send(animPacket.Write());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);

            C_Attacked attackedPacket = new C_Attacked();
            attackedPacket.playerId = PlayerId;
            attackedPacket.damage = 20;
            _network.Send(attackedPacket.Write());
        }
    }
}
