using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace �𶯼��ϵͳ
{

    public partial class Form_SP_Test : Form
    {
        static bool flag1 = false;  //�����������߳��źŵ�
        
        public Form_SP_Test()
        {
            InitializeComponent();
        }

        //��ʼ���԰�ť    ���
        public void Button_sp_test_Click(object sender, EventArgs e)
        {
            button_sp_test_close.Focus();//��������ֹͣ���԰�ť
            button_sp_test_close.Enabled = true;//ֹͣ���԰�ť    ����
            button_sp_tests_start.Enabled = false;//��ʼ���԰�ť  ����

            //��ʼ��ȡ���������߳�
            Thread readData = new Thread(ReadSpData);//ʵ��һ����ȡ�����߳�
            readData.IsBackground = true;//����Ϊ��̨�߳�
            readData.Priority = ThreadPriority.Highest;//�����߳����ȼ�Ϊ��
            flag1 = true;//�������߳��źŵ�ͨ��
            readData.Start(4);//��ʼ�߳�

            SendSpData("A");//����λ�����Ϳ�ʼ���������ź�
        }

        //���Ӳ��Դ���    ����
        public void Form_SP_Test_Shown(object sender, EventArgs e)
        {
            button_sp_tests_start.Focus();//�������ڿ�ʼ���԰�ť
            button_sp_test_close.Enabled = false;//ֹͣ���԰�ť    ����
            boxtest_com_num.Text = controlconfig.valueItem("portnum");
            boxtest_bode_num.Text = controlconfig.valueItem("bodenum");
        }

        //ֹͣ���԰�ť    ���
        private void button_sp_test_close_Click(object sender, EventArgs e)
        {
            flag1 = false;//ֹͣ�������߳�
            button_sp_test_close.Enabled = false;
            button_sp_tests_start.Enabled = true;
            //this.Close();
        }


        //���Ӳ��Դ���    �ر���
        private void Form_SP_Test_FormClosing(object sender, FormClosingEventArgs e)
        {
            flag1 = false;//ֹͣ�������߳�
        }

        //���ʹ�������
        public void SendSpData(string sendData)
        {
            CTSerialPort.SendSP(sendData);
        }

        //��ȡ��������    ��ʼ֮ǰҪ�����źŵ�ͨ��
        public void ReadSpData(object readSize)
        {
            byte[] spData = new byte[4];
            int num = 0;

            CTSerialPort.ClearInBuffer();
            boxtest_result.Text = null;
            while (flag1)
            {
                if (CTSerialPort.ReadSP(ref spData, (int)readSize))
                {
                    for (int n = 0; n < 4; n++)
                    {
                        boxtest_result.AppendText(spData[n].ToString() + " ");
                        //boxtest_result.Text += spData[n].ToString();
                        //this.boxtest_result.Focus();//��ȡ����
                        //this.boxtest_result.Select(this.boxtest_result.TextLength, 0);//��궨λ���ı����
                        //this.boxtest_result.ScrollToCaret();//��������괦
                    }
                    //boxtest_result.Text += "  ";
                    //this.boxtest_result.Focus();//��ȡ����
                    //this.boxtest_result.Select(this.boxtest_result.TextLength, 0);//��궨λ���ı����
                    //this.boxtest_result.ScrollToCaret();//��������괦
                    System.DateTime currentTime = new System.DateTime();
                    currentTime = System.DateTime.Now;
                    Console.WriteLine("Min:" + currentTime.Minute.ToString() + "  Sec:" + currentTime.Second.ToString()
                        + "  MsecL" + currentTime.Millisecond.ToString() + "\r\n");
                    num++;
                }
                if (num >= 10)
                {
                    boxtest_result.Text += "\r\n���Գɹ�\r\n";
                    flag1 = false;
                    SendSpData("E");
                    return;
                }
            }
            boxtest_result.Text += "\r\n����ʧ�ܣ�δ���յ���������\r\n";
            SendSpData("E");
            return;
        }
    }
}
