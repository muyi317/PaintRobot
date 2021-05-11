using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace PaintRobot
{
    public class SocketClient
    {
        private string _ip = string.Empty;
        private int _port = 0;
        private Socket _socket = null;
        private byte[] buffer = new byte[1024 * 1024 * 2];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">连接服务器的IP</param>
        /// <param name="port">连接服务器的端口</param>
        public SocketClient(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }
        public SocketClient(int port)
        {
            this._ip = "127.0.0.1";
            this._port = port;
        }

        /// <summary>
        /// 开启服务,连接服务端
        /// </summary>
        public string StartClient()
        {
            string message;
            try
            {
                //1.0 实例化套接字(IP4寻址地址,流式传输,TCP协议)
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //2.0 创建IP对象
                IPAddress address = IPAddress.Parse(_ip);
                //3.0 创建网络端口包括ip和端口
                IPEndPoint endPoint = new IPEndPoint(address, _port);
                //4.0 建立连接
                _socket.Connect(endPoint);
                message = "连接成功";
                _socket.ReceiveTimeout = 1000;
            }
            catch (SocketException ex)
            {
                // _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                Console.WriteLine(ex.Message);
                message = "连接失败"+ ex.Message;
            }
            return message;
        }

        public void stopClient()
        {
            _socket.Close();
        }

        public void sendData(string data)
        {
            //6.0 像服务器发送消息
            _socket.Send(Encoding.UTF8.GetBytes(data));
        }

        public string reciveData()
        {
            int length = 0;
            string message = "";
            try
            {
                //5.0 接收数据
                length = _socket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
            }
            catch (System.Net.Sockets.SocketException)
            {

            }

            if (length > 0)
            {
                Thread.Sleep(500);
                length = _socket.Receive(buffer);
                message = message + Encoding.UTF8.GetString(buffer, 0, length);
                return message; 
            }
            else
            {
                return "";
            }
        }
    }
}
    