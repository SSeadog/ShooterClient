using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PacketHandler
{
    // {패킷이름}Handler 요청하면 됩니다 라고 약속
    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.EnterGame(pkt);
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.LeaveGame(pkt);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Add(pkt);
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove pkt = packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Move(pkt);
    }

    public static void S_BroadcastFireHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastFire pkt = packet as S_BroadcastFire;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Fire(pkt);
    }

    public static void S_BroadcastAnimateHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastAnimate pkt = packet as S_BroadcastAnimate;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Animate(pkt);
    }

    public static void S_BroadcastAttackedHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastAttacked pkt = packet as S_BroadcastAttacked;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Attacked(pkt);
    }

    public static void S_BroadcastDieHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastDie pkt = packet as S_BroadcastDie;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Die(pkt);
    }
    
    public static void S_BroadcastRespawnHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastRespawn pkt = packet as S_BroadcastRespawn;
        ServerSession serverSession = session as ServerSession;

        PlayerManager.Instance.Respawn(pkt);
    }

    public static void S_TestReturnHandler(PacketSession session, IPacket packet)
    {
        S_TestReturn pkt = packet as S_TestReturn;
        ServerSession serverSession = session as ServerSession;

        Debug.Log($"SendTime: {pkt.startS} {pkt.startMS} ReceiveTime: {DateTime.Now.Second} {DateTime.Now.Millisecond}");
    }
}