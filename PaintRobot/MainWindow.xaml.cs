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

namespace PaintRobot
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketClient client;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            client = new SocketClient("192.168.1.1", 5233);
            textConnectState.Text = client.StartClient();
            Form1_Load();
        }

        private void Btn_send_Click(object sender, RoutedEventArgs e)
        {
            client.sendData(text_send.Text);
            text_send.Text = "";
        }
        private void Form1_Load()
        {
            
            //设置定时器          
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(1000);   //时间间隔为一秒
            timer.Tick += new EventHandler(reciveData);
            //开启定时器          
            timer.Start();
        }

        public void reciveData(object sender, EventArgs e)
        {
            Thread.Sleep(500);
            text_recive.Text = client.reciveData();
        }
    }
}
