using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


/* 机械臂控制流程的主程序
 * 1、通过传送喷涂件型号、尺寸，调用FANUC中对应名称的TP程序
 * 2、等待FANUC回传两个坐标点，计算变换矩阵
 * 3、变换矩阵发送给FANUC，FANUC运行修正后的喷涂轨迹
 * 4、继续等待FANUC回传两个坐标点，计算变换矩阵
 * 5、下发变换矩阵
 * 6、等待stop信号*/

namespace PaintRobot
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SocketClient m_client;
        public string m_type;
        public int m_column;
        public int m_raw;
        public int m_length;
        public int m_width;
        private Painting paint;

        public MainWindow()
        {
            InitializeComponent();
            
        }
        // 与FANUC建立通讯
        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            m_client = new SocketClient("192.168.1.1", 5233);
            textConnectState.Text = m_client.StartClient();
        }

        // 向机械臂发送消息
        private void Btn_send_Click(object sender, RoutedEventArgs e)
        {
            m_client.sendData(text_send.Text);
            text_send.Text = "";
        }

        // 接收机械臂发送的消息
        public void Btn_recive_Click(object sender, EventArgs e)
        {
            text_recive.Text = m_client.reciveData();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            paint = new Painting("DONGFENG", 0, 0, 0, 0, 0);
            paint.startPaint();
        }
    }
}
