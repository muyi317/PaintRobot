using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

/* 机械臂控制流程的主程序
 * 1、通过传送喷涂件型号、尺寸，调用FANUC中对应名称的TP程序
 * 2、等待FANUC回传两个坐标点，计算变换矩阵
 * 3、变换矩阵发送给FANUC，FANUC运行修正后的喷涂轨迹
 * 4、继续等待FANUC回传两个坐标点，计算变换矩阵
 * 5、下发变换矩阵
 * 6、等待stop信号*/

namespace PaintRobot
{
    class Painting
    {
        private SocketClient m_client;
        public string m_type = "DONGFENG";
        public int m_workPosition = 0;
        public int m_column = 0;
        public int m_raw = 0;
        public int m_length = 0;
        public int m_width = 0;

        public Painting(string type, int workPosition, int column, int raw, int length, int width)
        {
            m_type = type;
            m_workPosition = workPosition;
            m_column = column;
            m_raw = raw;
            m_length = length;
            m_width = width;
        }

        // 机械臂动作流程
        public void startPaint()
        {
            // 2、通过传送喷涂件型号、尺寸，调用FANUC中对应名称的TP程序
            string paintMessage = "TP" + m_type + "," + m_workPosition + "," + m_column + "," + m_raw + "," + m_length + "," + m_width + "," + "\n";
            sendMessage(paintMessage);

            while (true)
            {
                // 3、等待FANUC传入两个坐标点,计算变换矩阵并发送
                string message = reciveMessage();
                // MessageBox.Show(message);

                // FANUC发送stop时退出循环
                if (message == "stop")
                {
                    break;
                }

                string[] point = message.Split(';') ;
                double[,] point1 = new double[,] { { 0, 0, 0 }, { 0, 0, 0 } };
                double[,] point2 = new double[,] { { 0, 0, 0 }, { 0, 0, 0 } };

                for(int i=0; i<2; i++)
                {
                    for(int j=0; j<3; j++)
                    {
                        point1[i, j] = Convert.ToDouble(point[0].Split(',')[i * 3 + j]);
                        point2[i, j] = Convert.ToDouble(point[1].Split(',')[i * 3 + j]);
                    }
                }

                Transformer trans = new Transformer(point1, point2);
                double[,] transformMatrix = trans.m_transformMatrix.ToArray();                // 变换矩阵

                string transformMatrixStr = "";
                for(int i=0; i<4; i++)
                {
                    for(int j=0; j<4; j++)
                    {
                        transformMatrixStr = transformMatrixStr + transformMatrix[j, i] + ",";
                    }
                }

                // 4、发送变换矩阵
                sendMessage(transformMatrixStr);                                               // 将变换矩阵发送到FANUC
            }
        }


        // 与FANUC建立通讯
        public void connect()
        {
            m_client = new SocketClient("192.168.1.1", 5233);
            var linkStatus = m_client.StartClient();
        }

        // 断开FANUC通讯，结束FANUC端程序
        public void disconnect()
        {
            m_client.stopClient();
        }

        #region 私有函数

      

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
        #endregion
    }
}
