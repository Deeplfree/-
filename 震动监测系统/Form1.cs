using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace �𶯼��ϵͳ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //��������ʾ
        private void Form1_Shown(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            //��ʼ�账������״̬�Ĺ���
            {//�û���
                UserManage.Enabled = false;//�û�����ť ����
                UserLogOut.Enabled = false;//�˳���¼��ť ����
                UserSwitch.Enabled = false;//�л��û���ť ����
            }
            {//ͨ����
                SP_Set.Enabled = false;//�������ð�ť     ����
                SP_Ttest.Enabled = false;//���Ӳ��԰�ť   ����
                SP_Close.Enabled = false;//�Ͽ����Ӱ�ť   ����
            }

            //���ڿؼ�ˢ��
            Thread formreflash = new Thread(Form1Reflash);
            formreflash.IsBackground = true;
            formreflash.Start();
        }

        int xxxxxxx = 0;
        //�ؼ�ˢ��
        void Form1Reflash()
        {
            while (true)
            {
                Thread.Sleep(500);
                xxxxxxx++;
                //Console.WriteLine("From1 Reflash:{0}", xxxxxxx);
                /******************�û���¼������ԱȨ�޼��***************************************************************/
                if (CTMySql.isSignIn)   //�û��ѵ�¼ʱ
                {
                    UserLogOut.Enabled = true;//�˳���¼��ť ����
                    UserSwitch.Enabled = true;//�л��û���ť ����
                    UserSignIn.Enabled = false;//�û���¼��ť ����

                    SP_Set.Enabled = true;//�������ð�ť ����
                }
                else    //�û�δ��¼ʱ
                {
                    UserManage.Enabled = false;//�û�����ť ����
                    UserLogOut.Enabled = false;//�˳���¼��ť ����
                    UserSwitch.Enabled = false;//�л��û���ť ����
                    UserSignIn.Enabled = true;//�û���¼��ť ����

                    SP_Set.Enabled = false;//�������ð�ť     ����
                }
                if (CTMySql.isUserAdmin)    //�û��ǹ���Ա�û�ʱ
                {
                    UserManage.Enabled = true;//�û�����ť ����
                }
                else    //�û��ǹ���Ա�û�ʱ
                {
                    UserManage.Enabled = false;//�û�����ť ����
                }
                /****************���ڿ������********************************************************************************/
                if (CTSerialPort.IsComOpen())   //���ڿ���ʱ
                {
                    this.SP_Ttest.Enabled = true;//���Ӳ��԰�ť  ����
                    this.SP_Close.Enabled = true;//�Ͽ����Ӱ�ť  ����
                }
                else//���ڹر�ʱ
                {
                    this.SP_Ttest.Enabled = false;//���Ӳ��԰�ť  ����
                    this.SP_Close.Enabled = false;//�Ͽ����Ӱ�ť  ����
                }
            }
        }

        //������
        private void Form1_MouseEnter(object sender, EventArgs e)
        {

        }

        //�������ð�ť    ���
        private void SP_Set_Click(object sender, EventArgs e)
        {
            Form_SP_Set form_SP_Set = new Form_SP_Set();
            form_SP_Set.ShowDialog();
        }
        
        //�������Ӱ�ť    ���
        private void SP_Ttest_Click(object sender, EventArgs e)
        {
            Form_SP_Test form_sp_test = new Form_SP_Test();
            form_sp_test.ShowDialog();
        }

        //�Ͽ����Ӱ�ť    ���
        private void SP_Close_Click(object sender, EventArgs e)
        {
            CTSerialPort.CloseSP();
        }

        //�û���¼��ť    ���
        private void UserSignIn_Click(object sender, EventArgs e)
        {
            FormSignIn formSignIn = new FormSignIn();
            formSignIn.ShowDialog();
        }

        //�˳���¼��ť    ���
        private void UserLogOut_Click(object sender, EventArgs e)
        {
            CTMySql.isSignIn = false;
            CTMySql.isUserAdmin = false;
        }

        //�л��û���ť    ���
        private void UserSwitch_Click(object sender, EventArgs e)
        {
            CTMySql.isSignIn = false;
            FormSignIn formSignIn = new FormSignIn();
            formSignIn.ShowDialog();
        }

        //�û�����ť    ���
        private void UserManage_Click(object sender, EventArgs e)
        {
            CloseAllMdiForms();
            if (CTMySql.isUserAdmin) 
            {
                FormUserManage fum = new FormUserManage();
                fum.MdiParent = this;
                fum.WindowState = FormWindowState.Maximized;
                fum.Show();
            }
            else
            {
                MessageBox.Show("���û�û�й���Ȩ��");
            }
        }

        //ʵʱ���ݰ�ť    ���
        private void ButtonRealData_MyButtonClickEvent(object sender, EventArgs e)
        {
            CloseAllMdiForms();
            FormWave fw = new FormWave();
            Console.WriteLine("FormWave RealData have show.");
            fw.MdiParent = this;
            fw.WindowState = FormWindowState.Maximized;
            fw.Show();
        }

        //��ʷ���ݰ�ť    ���
        private void ButtonHistoryData_MyButtonClickEvent(object sender, EventArgs e)
        {
            CloseAllMdiForms();
            FormHistoryData fh = new FormHistoryData();
            Console.WriteLine("FormHistoryData have show.");
            fh.MdiParent = this;
            fh.WindowState = FormWindowState.Maximized;
            fh.Show();
        }

        //ͨ�����ð�ť    ���
        private void ButtonSeriportSet_MyButtonClickEvent(object sender, EventArgs e)
        {
            CloseAllMdiForms();
            Form_SP_Test fspt = new Form_SP_Test();
            fspt.MdiParent = this;
            fspt.WindowState = FormWindowState.Normal;
            fspt.Show();
            Form_SP_Set fsps = new Form_SP_Set();
            fsps.MdiParent = this;
            fsps.WindowState = FormWindowState.Normal;
            fsps.Show();
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        void CloseAllMdiForms()
        {
            if (this.MdiChildren.Length != 0)
            {
                int n = this.MdiChildren.Length;
                for (int i = 0; i < n; i++)
                {
                    this.MdiChildren[0].Close();
                }
            }
        }
    }
}
