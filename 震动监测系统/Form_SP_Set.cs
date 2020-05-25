using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace �𶯼��ϵͳ
{
    public partial class Form_SP_Set : Form
    {
        public Form_SP_Set()
        {
            InitializeComponent();
            opencom_button.Focus();
            opencom_button.Focus();
        }

        
        private void Form_SP_Set_Shown(object sender, EventArgs e)
        {
            //�˿ں�ѡ��
            string[] str = SerialPort.GetPortNames();
            if (str == null)
                MessageBox.Show("δ��⵽����", "����ѡ�����");

            foreach (string com in SerialPort.GetPortNames())
            {
                com_list.Items.Add(com);
            }
            if (controlconfig.existItem("portnum"))
                if (controlconfig.valueItem("portnum") != null)
                {
                    com_list.Text = controlconfig.valueItem("portnum");
                }
                else
                    com_list.SelectedIndex = 0;
            else
                //com_list.SelectedIndex = 0;
                com_list.Text = "";

            //������ѡ��
            int[] bode = { 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200 };
            for (int x = 0; x < 8; x++)
            {
                bode_list.Items.Add(bode[x]);
            }
            if (controlconfig.existItem("bodenum"))
                bode_list.Text = controlconfig.valueItem("bodenum");
            else 
                bode_list.SelectedIndex = 0;

            //����Ĭ������λ��ֹͣλ
            if (controlconfig.existItem("datanum"))
                data_box.Text = controlconfig.valueItem("datanum");
            else
                data_box.Text = "8";
            if (controlconfig.existItem("stopnum"))
                stop_box.Text = controlconfig.valueItem("stopnum");
            else
                stop_box.Text = "1";
        }

        //�򿪶˿ڰ�ť
        private void Opencom_button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            bool a, b;
            a = CTSerialPort.SetSP(com_list.Text, bode_list.Text, data_box.Text, stop_box.Text);
            b = CTSerialPort.OpenSP();
            if (a && b)
            {
                MessageBox.Show("�򿪳ɹ�");
                //���洮�ڲ���
                controlconfig.AddOrModifyItem("portnum", com_list.Text);
                controlconfig.AddOrModifyItem("bodenum", bode_list.Text);
                controlconfig.AddOrModifyItem("datanum", data_box.Text);
                controlconfig.AddOrModifyItem("stopnum", stop_box.Text);

                if (btn.FindForm().IsMdiChild)
                    return;
                this.Close();
                Form_SP_Test form_sp_test = new Form_SP_Test();
                form_sp_test.ShowDialog();
                return;
            }
            return;
        }
    }
}
