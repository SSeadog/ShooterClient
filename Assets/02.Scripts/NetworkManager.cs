using DummyClient;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    ServerSession _session = new ServerSession();

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }

    void Start()
    {
        // DNS
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = IPAddress.Parse("210.179.36.63");
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();

        connector.Connect(endPoint,
            () => { return _session; },
            1);

        Debug.Log(connector.ToString());
    }

    void Update()
    {
        List<IPacket> list = PacketQueue.Instance.PopAll();
        foreach (IPacket packet in list)
        {
            PacketManager.Instance.HandlePacket(_session, packet);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
        Debug.Log("disconnect");
        C_LeaveGame leavePacket = new C_LeaveGame();
        _session.Send(leavePacket.Write());
        _session.Disconnect();
        PlayerManager.Instance.LeaveGame();
    }

    void OnApplicationQuit()
    {
        if (!GameManager.Instance.serverConnected)
        {
            return;
        }

        QuitGame();
    }
}
