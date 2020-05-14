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
    public partial class FormSignIn : Form
    {
        CTMySql cTMySql;

        public FormSignIn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �Ƿ��¼�źŵ�
        ///</summary>
        public bool signInFlag { get; set; }
        public bool adminFlag { get; set; }

        static Form1 f1 = new Form1();
        static Thread f1t = new Thread(F1Show);
        //��¼��ť  ���
        private void btmSignIn_Click(object sender, EventArgs e)
        {
            string userAccount = txtbxAccount.Text;
            string userPassword = txtbxPassword.Text;
            bool a = cTMySql.CheckSignInPassword(userAccount, userPassword);//�ж������Ƿ���ȷ
            if (a)
            {
                MessageBox.Show("��½�ɹ�");
                CTMySql.signInUserName = txtbxAccount.Text;
                if (f1t.IsAlive)
                {
                    f1.CloseAllMdiForms();
                    this.Close();
                    return;
                }
                else
                {
                    f1t.IsBackground = false;
                    f1t.Start();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("��½ʧ��");
            }
        }

        static void F1Show()
        {
            f1.ShowDialog();
        }

        //��½ҳ��  �״γ���
        private void FormSignIn_Shown(object sender, EventArgs e)
        {
            cTMySql = new CTMySql("localhost", "root", "�𶯼��ϵͳ", "000000", "3306");//�������Ӳ���
            CTMySql.isSignIn = false;
            if (cTMySql.ConnectDatabass())//�����ӣ��ж������Ƿ�ɹ�
            {
                //MessageBox.Show("Success To Connect MySQL");
            }
            else
            {
                MessageBox.Show("Fail To Connect MySQL");
            }
        }

        //�û��������    ��������
        private void txtbxAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            //�ж��û��������Ƿ�Ϸ�
            Byte[] a = { 0 };
            Encoding.ASCII.GetBytes(e.KeyChar.ToString(), 0, 1, a, 0);//�������ַ�תΪASCII�룬��������a
            Console.WriteLine("keychar:" + e.KeyChar);
            Console.WriteLine("keyascii:" + a[0]);
            Console.WriteLine();
            int x = a[0];
            //�ж��ַ��Ƿ�Ϸ�
            if (
                   (x >= 65 && x <= 90)//�����д��ĸ
                   || (x >= 97 && x <= 122)//����Сд��ĸ
                   || (x >= 48 && x <= 57)//��������
                   || (x == 8)//�����˸�
                   || (x == 32)//����ո�
                   //|| (x == 46)//����С����
                   || (x == 95)//�����»���
               )
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                
            }
        }

        //���������     ��������
        private void txtbxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            //�ж����������Ƿ�Ϸ�
            Byte[] a = { 0 };
            Encoding.ASCII.GetBytes(e.KeyChar.ToString(), 0, 1, a, 0);//�������ַ�תΪASCII�룬��������a
            Console.WriteLine("keychar:" + e.KeyChar);
            Console.WriteLine("keyascii:" + a[0]);
            Console.WriteLine();
            int x = a[0];
            //�ж��ַ��Ƿ�Ϸ�
            if (
                   (x >= 65 && x <= 90)//�����д��ĸ
                   || (x >= 97 && x <= 122)//����Сд��ĸ
                   || (x >= 48 && x <= 57)//��������
                   || (x == 8)//�����˸�
                   //|| (x == 32)//����ո�
                   || (x == 46)//����С����
                   || (x == 95)//�����»���
               )
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
