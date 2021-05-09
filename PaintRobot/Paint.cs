using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 机械臂控制流程的主程序
 * 1、通过传送喷涂件型号、尺寸，调用FANUC中对应名称的TP程序
 * 2、等待FANUC回传两个坐标点，计算变换矩阵
 * 3、变换矩阵发送给FANUC，FANUC运行修正后的喷涂轨迹
 * 4、继续等待FANUC回传两个坐标点，计算变换矩阵
 * 5、下发变换矩阵
 * 6、等待stop信号*/

namespace PaintRobot
{
    class Paint
    {
        private SocketClient m_client;
        public string m_type;
        public int m_column;
        public int m_raw;
        public int m_length;
        public int m_width;

        // 与FANUC建立通讯
        private void connect()
        {
            m_client = new SocketClient("192.168.1.1", 5233);
            var linkStatus = m_client.StartClient();
        }

        // 向机械臂发送消息
        private void sendMessage(string message)
        {
            m_client.sendData(message);
        }

        // 接收机械臂发送的消息
        private string reciveMessage()
        {
            return m_client.reciveData();
        }

        public void startPaint()
        {
            // 1、建立连接
            connect();

            // 2、通过传送喷涂件型号、尺寸，调用FANUC中对应名称的TP程序
            string paintMessage = "TP" + m_type + "," + m_column + "," + m_raw + "," + m_length + "," + m_width;
            sendMessage(paintMessage);

            // 3、等待
        }
    }
}
