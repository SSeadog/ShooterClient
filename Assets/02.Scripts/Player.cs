using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    protected NetworkManager _network;

    public bool isDie;
    public int PlayerId { get; set; }
    public int Hp;
    public Vector3 Destination; // 서버에서 받은 좌표
    public Quaternion RotationDestination; // 서버에서 받은 회전값

    protected Transform firePos;
    protected GameObject bulletPrefab;

    public RectTransform Hpbar;

    public enum AnimState
    {
        idle = 0,
        runForward,
        runBackward,
        runRight,
        runLeft
    }

    public AnimState animState = AnimState.idle;
    public AnimationClip[] animClips;

    protected CharacterController controller;
    protected Animation anim;

    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        isDie = false;

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
        anim.CrossFade(animClips[(int)animState].name, 0.2f);
    }

    public void Fire()
    {
        //GameObject.Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        C_Fire firePacket = new C_Fire();
        firePacket.posX = firePos.position.x;
        firePacket.posY = firePos.position.y;
        firePacket.posZ = firePos.position.z;
        firePacket.rotY = firePos.rotation.eulerAngles.y;

        DateTime now = DateTime.Now;
        string fireTime = now.Second.ToString("D2") + now.Millisecond.ToString();
        firePacket.startTime = fireTime;

        _network.Send(firePacket.Write());
    }

    public void Fire(Vector3 firePos, Quaternion fireRot)
    {
        //GameObject.Instantiate(bulletPrefab, firePos, fireRot);
        
    }

    public void Respawn()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
        }
    }
}
