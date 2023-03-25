using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 全局弹幕连接
    /// </summary>
    public static class DConnection
    {
        private static BiliWebSocket _ws = new BiliWebSocket();
        public static BiliWebSocket BiliWS => _ws;
        /// <summary>
        /// 当前连接状态
        /// </summary>
        public static ConnectState State { get; private set; }
        /// <summary>
        /// 连接到指定直播间
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="onConnected"></param>
        public static async void ConnectAsync(string roomId, Action<int, string> onConnected)
        {
            if (State == ConnectState.Connected && State == ConnectState.Connecting) return;
            State = ConnectState.Connecting;
            _ws.ConnectAsync(roomId, (code, msg) =>
            {
                if(code == 0)
                {
                    State = ConnectState.Connected;
                }
                else
                {
                    State = ConnectState.Disconnected;
                }
                onConnected?.Invoke(code, msg);
            });
        }

        public static void Disconnect()
        {
            if (State == ConnectState.Connected || State == ConnectState.Connecting)
            {
                _ws.DisConnect();
                State = ConnectState.Disconnected;
            }
        }

        public enum ConnectState
        {
            Disconnected,
            Connecting,
            Connected,
        }
    }
}
