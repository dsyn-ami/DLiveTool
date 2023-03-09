using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLiveTool
{
    public struct Packet
    {
        /// <summary>
        /// 心跳包
        /// </summary>
        public static readonly Packet HeatBeat = new Packet()
        {
            Header = new PacketHeader()
            {
                _headerLength = PacketHeader._packetHeaderLength,
                _sequenceId = 1,
                _protocolVersion = ProtocolVersion.HeartBeat,
                _operation = Operation.HeartBeat,
                _packetLength = PacketHeader._packetHeaderLength
            }
        };

        public PacketHeader Header;

        public int Length => Header._packetLength;

        public byte[] Body;
        public Packet(byte[] bytes)
        {
            Header = new PacketHeader(bytes.Take(PacketHeader._packetHeaderLength).ToArray());
            byte[] body = bytes.Skip(PacketHeader._packetHeaderLength).ToArray();
            Body = body;
        }
        public Packet(Operation operation, byte[] body = null)
        {
            Header = new PacketHeader
            {
                _operation = operation,
                _protocolVersion = ProtocolVersion.UnCompressed,
                _packetLength = PacketHeader._packetHeaderLength + (body?.Length ?? 0),
                _headerLength = PacketHeader._packetHeaderLength,
                _sequenceId = 1
            };
            Body = body;
        }

        public byte[] ToBytes()
        {
            if (Body != null)
                Header._packetLength = Header._headerLength + Body.Length;
            else
                Header._packetLength = Header._headerLength;
            var arr = new byte[Header._packetLength];
            Array.Copy(Header.ToBytes(), arr, Header._headerLength);
            if (Body != null)
                Array.Copy(Body, 0, arr, Header._headerLength, Body.Length);
            return arr;
        }

        /// <summary>
        /// 生成附带msg信息的心跳包
        /// </summary>
        /// <param name="msg">需要带的信息</param>
        /// <returns>心跳包</returns>
        public static Packet HeartBeat(string msg)
        {
            return HeartBeat(Encoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// 生成附带msg信息的心跳包
        /// </summary>
        /// <param name="msg">需要带的信息</param>
        /// <returns>心跳包</returns>
        public static Packet HeartBeat(byte[] msg = null)
        {
            if (msg == null) return HeatBeat;
            return new Packet()
            {
                Header = new PacketHeader()
                {
                    _packetLength = PacketHeader._packetHeaderLength + msg.Length,
                    _protocolVersion = ProtocolVersion.HeartBeat,
                    _operation = Operation.HeartBeat,
                    _sequenceId = 1,
                    _headerLength = PacketHeader._packetHeaderLength
                },
                Body = msg
            };
        }

        /// <summary>
        /// 生成验证用数据包
        /// </summary>
        /// <param name="token">http请求获取的token</param>
        /// <param name="protocolVersion">协议版本</param>
        /// <returns>验证请求数据包</returns>
        public static Packet Authority(string token,
            ProtocolVersion protocolVersion = ProtocolVersion.Brotli)
        {
            byte[] obj = Encoding.UTF8.GetBytes(token);

            return new Packet
            {
                Header = new PacketHeader
                {
                    _operation = Operation.Authority,
                    _protocolVersion = ProtocolVersion.HeartBeat,
                    _sequenceId = 1,
                    _headerLength = PacketHeader._packetHeaderLength,
                    _packetLength = PacketHeader._packetHeaderLength + obj.Length
                },
                Body = obj
            };
        }
    }

    /// <summary>
    /// 弹幕数据包头部
    /// </summary>
    public struct PacketHeader
    {
        public const int _packetHeaderLength = 16;

        public int _packetLength;
        public short _headerLength;
        public ProtocolVersion _protocolVersion;
        public Operation _operation;
        public int _sequenceId;

        public int _bodyLength => _packetLength - _headerLength;


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="bytes">弹幕头16字节</param>
        public PacketHeader(byte[] bytes)
        {
            if (bytes.Length != _packetHeaderLength) throw new ArgumentException("No Supported Protocol Header");

            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
                _packetLength = BitConverter.ToInt32(bytes, 12);
                _headerLength = BitConverter.ToInt16(bytes, 10);
                _protocolVersion = (ProtocolVersion)BitConverter.ToInt16(bytes, 8);
                _operation = (Operation)BitConverter.ToInt32(bytes, 4);
                _sequenceId = BitConverter.ToInt32(bytes, 0);
            }
            else
            {
                _packetLength = BitConverter.ToInt32(bytes, 0);
                _headerLength = BitConverter.ToInt16(bytes, 4);
                _protocolVersion = (ProtocolVersion)BitConverter.ToInt16(bytes, 6);
                _operation = (Operation)BitConverter.ToInt32(bytes, 8);
                _sequenceId = BitConverter.ToInt32(bytes, 12);
            }
            
        }

        /// <summary>
        /// 获得头部的 byte 数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            var bytes = new byte[_packetHeaderLength];
            var pl = BitConverter.GetBytes(_packetLength);
            var hl = BitConverter.GetBytes(_headerLength);
            var pv = BitConverter.GetBytes((short)_protocolVersion);
            var ot = BitConverter.GetBytes((int)_operation);
            var si = BitConverter.GetBytes(_sequenceId);

            //如果当前系统是小端排序，即低位字节存储在内存的低地址端，高位字节存储在内存的高地址端。
            //那么应该将 byte数组 反转后使用
            //如 Int32(258) 小端排序系统中转换成 byte数组 是 [2, 1, 0, 0]
            //必要时需要反转成 [0, 0, 1, 2] 以便使用
            //大多数系统都是小端排序
            if (BitConverter.IsLittleEndian)
            {
                pl = pl.Reverse().ToArray();
                hl = hl.Reverse().ToArray();
                pv = pv.Reverse().ToArray();
                ot = ot.Reverse().ToArray();
                si = si.Reverse().ToArray();
            }
            Array.Copy(pl, 0, bytes, 0, pl.Length);
            Array.Copy(hl, 0, bytes, pl.Length, hl.Length);
            Array.Copy(pv, 0, bytes, pl.Length + hl.Length, pv.Length);
            Array.Copy(ot, 0, bytes, pl.Length + hl.Length + pv.Length, ot.Length);
            Array.Copy(si, 0, bytes, pl.Length + hl.Length + pv.Length + ot.Length, si.Length);
            return bytes;
        }
    }

    /// <summary>
    /// 操作数据
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// 心跳包
        /// </summary>
        HeartBeat = 2,

        /// <summary>
        /// 服务器心跳回应(包含人气信息)
        /// </summary>
        HeartBeatResponse = 3,

        /// <summary>
        /// 服务器消息(正常消息)
        /// </summary>
        ServerNotify = 5,

        /// <summary>
        /// 客户端认证请求
        /// </summary>
        Authority = 7,

        /// <summary>
        /// 认证回应
        /// </summary>
        AuthorityResponse = 8
    }

    /// <summary>
    /// 弹幕协议版本
    /// </summary>
    public enum ProtocolVersion
    {
        /// <summary>
        /// 未压缩数据
        /// </summary>
        UnCompressed = 0,

        /// <summary>
        /// 心跳数据
        /// </summary>
        HeartBeat = 1,

        /// <summary>
        /// zlib数据
        /// </summary>
        Zlib = 2,

        /// <summary>
        /// Br数据
        /// </summary>
        Brotli = 3
    }
}