using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;

    MyPlayer _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance;

    void Start()
    {
        Instance = gameObject.GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (!GameManager.Instance.serverConnected)
        {
            return;
        }

        foreach (Player p in _players.Values)
        {
            if (p.Destination == null)
                return;

            p.transform.position = Vector3.Lerp(p.transform.position, p.Destination, 0.005f);
        }  
    }

    public void Add(S_PlayerList packet)
    {
        foreach (S_PlayerList.Player p in packet.players)
        {
            if (p.isSelf)
            {
                GameObject myObj = Object.Instantiate(playerPrefab) as GameObject;
                MyPlayer myPlayer = myObj.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _myPlayer = myPlayer;
            }
            else
            {
                GameObject go = Object.Instantiate(playerPrefab) as GameObject;
                Player player = go.AddComponent<Player>();
                player.PlayerId = p.playerId;
                player.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _players.Add(p.playerId, player);
            }
        }
    }

    public void Move(S_BroadcastMove packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            _myPlayer.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.Destination = new Vector3(packet.posX, packet.posY, packet.posZ);
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

        GameObject go = Object.Instantiate(playerPrefab) as GameObject;

        Player player = go.AddComponent<Player>();
        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
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
