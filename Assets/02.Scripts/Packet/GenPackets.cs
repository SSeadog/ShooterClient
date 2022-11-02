using System;
using ServerCore;
using System.Net;
using System.Text;
using System.Collections.Generic;

public enum PacketID
{
    S_BroadcastEnterGame = 1,
    C_LeaveGame = 2,
    S_BroadcastLeaveGame = 3,
    S_PlayerList = 4,
    C_Move = 5,
    S_BroadcastMove = 6,
    C_Fire = 7,
    S_BroadcastFire = 8,
    C_Animate = 9,
    S_BroadcastAnimate = 10,
    C_Attacked = 11,
    S_BroadcastAttacked = 12,
    S_BroadcastDie = 13,
    S_BroadcastRespawn = 14,
    C_Test = 15,
    S_TestReturn = 16,

}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}

public class S_BroadcastEnterGame : IPacket
{
    public int playerId;
    public int hp;
    public float posX;
    public float posY;
    public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.hp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnterGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.hp), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class C_LeaveGame : IPacket
{


    public ushort Protocol { get { return (ushort)PacketID.C_LeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);


    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastLeaveGame : IPacket
{
    public int playerId;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastLeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastLeaveGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_PlayerList : IPacket
{
    public class Player
    {
        public bool isSelf;
        public int playerId;
        public int hp;
        public float posX;
        public float posY;
        public float posZ;

        public void Read(ArraySegment<byte> segment, ref ushort count)
        {
            this.isSelf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
            count += sizeof(bool);
            this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            this.hp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);
            this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);
            this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
            count += sizeof(float);

        }

        public bool Write(ArraySegment<byte> segment, ref ushort count)
        {
            bool success = true;
            Array.Copy(BitConverter.GetBytes(this.isSelf), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);
            Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(this.hp), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);
            Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);
            Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
            count += sizeof(float);

            return success;
        }
    }

    public List<Player> players = new List<Player>();


    public ushort Protocol { get { return (ushort)PacketID.S_PlayerList; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.players.Clear();
        ushort playerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += sizeof(ushort);
        for (int i = 0; i < playerLen; i++)
        {
            Player player = new Player();
            player.Read(segment, ref count);
            players.Add(player);
        }

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_PlayerList), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)this.players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        foreach (Player player in players)
        {
            player.Write(segment, ref count);
        }


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class C_Move : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotY;
    public float velX;
    public float velY;
    public float velZ;

    public ushort Protocol { get { return (ushort)PacketID.C_Move; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.rotY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.velX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.velY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.velZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Move), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.rotY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.velX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.velY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.velZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastMove : IPacket
{
    public int playerId;
    public float posX;
    public float posY;
    public float posZ;
    public float rotY;
    public float velX;
    public float velY;
    public float velZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastMove; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.rotY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.velX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.velY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.velZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastMove), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.rotY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.velX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.velY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.velZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class C_Fire : IPacket
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotY;
    public string startTime;

    public ushort Protocol { get { return (ushort)PacketID.C_Fire; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.rotY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        ushort startTimeLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += sizeof(ushort);
        this.startTime = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, startTimeLen);
        count += startTimeLen;

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Fire), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.rotY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        ushort startTimeLen = (ushort)Encoding.Unicode.GetBytes(this.startTime, 0, this.startTime.Length, segment.Array, segment.Offset + count + sizeof(ushort));
        Array.Copy(BitConverter.GetBytes(startTimeLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        count += startTimeLen;


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastFire : IPacket
{
    public int playerId;
    public float posX;
    public float posY;
    public float posZ;
    public float rotY;
    public string startTime;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastFire; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.rotY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        ushort startTimeLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += sizeof(ushort);
        this.startTime = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, startTimeLen);
        count += startTimeLen;

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastFire), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.rotY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        ushort startTimeLen = (ushort)Encoding.Unicode.GetBytes(this.startTime, 0, this.startTime.Length, segment.Array, segment.Offset + count + sizeof(ushort));
        Array.Copy(BitConverter.GetBytes(startTimeLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        count += startTimeLen;


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class C_Animate : IPacket
{
    public int animIndex;

    public ushort Protocol { get { return (ushort)PacketID.C_Animate; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.animIndex = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Animate), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.animIndex), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastAnimate : IPacket
{
    public int playerId;
    public int animIndex;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastAnimate; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.animIndex = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastAnimate), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.animIndex), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class C_Attacked : IPacket
{
    public int playerId;
    public int damage;

    public ushort Protocol { get { return (ushort)PacketID.C_Attacked; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.damage = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Attacked), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.damage), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastAttacked : IPacket
{
    public int playerId;
    public int hp;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastAttacked; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.hp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastAttacked), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.hp), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastDie : IPacket
{
    public int playerId;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastDie; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastDie), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastRespawn : IPacket
{
    public int playerId;
    public int hp;
    public float posX;
    public float posY;
    public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastRespawn; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.hp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
        count += sizeof(int);
        this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);
        this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
        count += sizeof(float);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastRespawn), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.hp), 0, segment.Array, segment.Offset + count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);
        Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
        count += sizeof(float);


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class C_Test : IPacket
{
    public string startS;
    public string startMS;

    public ushort Protocol { get { return (ushort)PacketID.C_Test; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        ushort startSLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += sizeof(ushort);
        this.startS = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, startSLen);
        count += startSLen;
        ushort startMSLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += sizeof(ushort);
        this.startMS = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, startMSLen);
        count += startMSLen;

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Test), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        ushort startSLen = (ushort)Encoding.Unicode.GetBytes(this.startS, 0, this.startS.Length, segment.Array, segment.Offset + count + sizeof(ushort));
        Array.Copy(BitConverter.GetBytes(startSLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        count += startSLen;
        ushort startMSLen = (ushort)Encoding.Unicode.GetBytes(this.startMS, 0, this.startMS.Length, segment.Array, segment.Offset + count + sizeof(ushort));
        Array.Copy(BitConverter.GetBytes(startMSLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        count += startMSLen;


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}
public class S_TestReturn : IPacket
{
    public string startS;
    public string startMS;

    public ushort Protocol { get { return (ushort)PacketID.S_TestReturn; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        ushort startSLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += sizeof(ushort);
        this.startS = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, startSLen);
        count += startSLen;
        ushort startMSLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += sizeof(ushort);
        this.startMS = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, startMSLen);
        count += startMSLen;

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;

        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_TestReturn), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        ushort startSLen = (ushort)Encoding.Unicode.GetBytes(this.startS, 0, this.startS.Length, segment.Array, segment.Offset + count + sizeof(ushort));
        Array.Copy(BitConverter.GetBytes(startSLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        count += startSLen;
        ushort startMSLen = (ushort)Encoding.Unicode.GetBytes(this.startMS, 0, this.startMS.Length, segment.Array, segment.Offset + count + sizeof(ushort));
        Array.Copy(BitConverter.GetBytes(startMSLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);
        count += startMSLen;


        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

