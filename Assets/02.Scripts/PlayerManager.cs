using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Player;
using static S_PlayerList;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;

    MyPlayer _myPlayer;
    CharacterController _myPlayerController;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance;
    GameObject bulletPrefab;

    void Start()
    {
        Instance = gameObject.GetComponent<PlayerManager>();

        bulletPrefab = (GameObject)Resources.Load("Bullet");
    }

    void Update()
    {
        if (!GameManager.Instance.serverConnected)
        {
            return;
        }

        foreach (Player p in _players.Values)
        {
            if (p.isDie)
                return;

            if (p.Destination == null)
                return;

            p.transform.position = Vector3.Lerp(p.transform.position, p.Destination, Time.deltaTime * 5.0f);
            p.transform.rotation = Quaternion.Slerp(p.transform.rotation, p.RotationDestination, Time.deltaTime * 5.0f);
        }  
    }

    public void Add(S_PlayerList packet)
    {
        foreach (S_PlayerList.Player p in packet.players)
        {
            if (p.isSelf)
            {
                GameObject myObj = GameObject.Instantiate(playerPrefab);
                MyPlayer myPlayer = myObj.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.Hp = p.hp;
                //myPlayer.Hpbar.localScale = new Vector2(p.hp / 100f, 1f);
                myPlayer.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _myPlayer = myPlayer;

                _myPlayerController = _myPlayer.GetComponent<CharacterController>();

                // 카메라에게 타겟 설정해주기
                GameObject.Find("Main Camera").GetComponent<CameraCtrl>().target = myObj;
            }
            else
            {
                GameObject go = GameObject.Instantiate(playerPrefab);
                Player player = go.AddComponent<Player>();
                player.PlayerId = p.playerId;
                player.Hp = p.hp;
                //player.Hpbar.localScale = new Vector2(p.hp / 100f, 1f);
                player.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _players.Add(p.playerId, player);
            }
        }
    }

    public void Move(S_BroadcastMove packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            //_myPlayer.Destination = new Vector3(packet.posX, packet.posY, packet.posZ);
            //_myPlayer.RotationDestination = Quaternion.Euler(packet.rotX, packet.rotY, packet.rotZ);
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                if (player.isDie)
                    return;

                player.Destination = new Vector3(packet.posX, packet.posY, packet.posZ) + new Vector3((float)(packet.velX * 0.1), (float)(packet.velY * 0.1), (float)(packet.velZ * 0.1));
                Vector3 playerRot = player.transform.eulerAngles;
                player.RotationDestination = Quaternion.Euler(playerRot.x, packet.rotY, playerRot.z);
            }
        }
    }

    public void Fire(S_BroadcastFire packet)
    {
        DateTime now = DateTime.Now;
        string fireRecvTime = now.Second.ToString("D2") + now.Millisecond.ToString();
        int latency = Int32.Parse(fireRecvTime) - Int32.Parse(packet.startTime);
        if (_myPlayer.PlayerId == packet.playerId)
        {
            if (_myPlayer.isDie)
                return;

            Quaternion fireRot = Quaternion.Euler(0, packet.rotY, 0);
            Vector3 firePos = new Vector3(packet.posX, packet.posY, packet.posZ) + (fireRot * Vector3.forward * 10f * latency * 0.001f);

            GameObject.Instantiate(bulletPrefab, firePos, fireRot);
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                if (player.isDie)
                    return;

                Vector3 firePos = new Vector3(packet.posX, packet.posY, packet.posZ);
                Quaternion fireRot = Quaternion.Euler(0, packet.rotY, 0);

                GameObject.Instantiate(bulletPrefab, firePos, fireRot);
            }
        }
    }

    public void Animate(S_BroadcastAnimate packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.animState = (Player.AnimState)packet.animIndex;
            }
        }
    }

    public void Attacked(S_BroadcastAttacked packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            _myPlayer.Hp = packet.hp;
            _myPlayer.Hpbar.localScale = new Vector2(packet.hp / 100f, 1f);

        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.Hp = packet.hp;
                player.Hpbar.localScale = new Vector2(packet.hp / 100f, 1f);
            }
        }
    }

    public void Die(S_BroadcastDie packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            _myPlayer.isDie = true;
            _myPlayer.Hpbar.localScale = new Vector2(0, 0);
            _myPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            _myPlayer.GetComponentInChildren<MeshRenderer>().enabled = false;
            _myPlayer.GetComponent<CharacterController>().enabled = false;
            _myPlayer.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.isDie = true;
                player.Hpbar.localScale = new Vector2(0, 0);
                player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                player.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    public void Respawn(S_BroadcastRespawn packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            _myPlayer.Hp = packet.hp;
            _myPlayer.Hpbar.localScale = new Vector2(packet.hp / 100f, 1f);
            Vector3 spawnPoint = new Vector3(packet.posX, packet.posY, packet.posZ);
            _myPlayer.transform.position = spawnPoint;
            _myPlayer.Destination = spawnPoint;
            _myPlayer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            _myPlayer.GetComponentInChildren<MeshRenderer>().enabled = true;
            _myPlayer.GetComponent<CharacterController>().enabled = true;
            _myPlayer.GetComponent<CapsuleCollider>().enabled = true;
            _myPlayer.isDie = false;
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.Hp = packet.hp;
                player.Hpbar.localScale = new Vector2(packet.hp / 100f, 1f);
                Vector3 spawnPoint = new Vector3(packet.posX, packet.posY, packet.posZ);
                player.transform.position = spawnPoint;
                player.Destination = spawnPoint;
                player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                player.GetComponentInChildren<MeshRenderer>().enabled = true;
                player.isDie = false;
            }
        }
    }

    public int GetPlayersCount()
    {
        // 자신까지 더해서 전달
        return _players.Count + 1;
    }

    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            return;
        }

        GameObject go = GameObject.Instantiate(playerPrefab);

        Player player = go.AddComponent<Player>();
        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        player.PlayerId = packet.playerId;
        player.Hp = packet.hp;
        _players.Add(packet.playerId, player);
    }

    public void LeaveGame()
    {
        GameObject.Destroy(_myPlayer.gameObject);
        _myPlayer = null;

        // 마이플레이어가 게임을 떠나면 다른 플레이어들도 삭제
        for (int i = 0; i < _players.Count; i++)
        {
            GameObject.Destroy(_players.ElementAt(i).Value.gameObject);
            _players.Remove(i);
        }
    }

    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
            
            // 마이플레이어가 게임을 떠나면 다른 플레이어들도 삭제
            for (int i = 0; i < _players.Count; i++)
            {
                GameObject.Destroy(_players[i].gameObject);
                _players.Remove(i);
            }
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player)) {
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }
}
