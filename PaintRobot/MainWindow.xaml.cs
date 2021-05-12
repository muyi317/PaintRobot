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
        public string m_type;
        public int m_workPos;
        public int m_column;
        public int m_raw;
        public int m_length;
        public int m_width;
        private Painting paint;

        public MainWindow()
        {
            InitializeComponent();

            /* m_type 工件型号
             * m_column 工件排列行数（横排多少个）
             * m_raw 工件排列列数（竖排多少个）
             * m_length 工件列间距（横排间距）
             * m_width 工件行间距（竖排间距）
             */
            m_type = "DONGFENG";
            m_workPos = 1;
            m_column = 2;
            m_raw = 3;
            m_length = 400;
            m_width = 50;

            // 实例化painting类
            paint = new Painting(m_type, m_workPos, m_column, m_raw, m_length, m_width);
        }

        // 与FANUC建立通讯
        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            // 链接FANUC
            paint.connect();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            // 开始绘图轨迹，运行此函数需要一定时间，完成后自动结束
            paint.startPaint();
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            // 断开链接并结束FANUC端程序
            paint.disconnect();
        }
    }
}
