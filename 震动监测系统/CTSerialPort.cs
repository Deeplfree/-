using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace �𶯼��ϵͳ
{
    static class CTSerialPort
    {
        
        static SerialPort sp = new SerialPort();

        /// <summary>
        /// ���ô���ͨ�Ų���
        /// </summary>
        /// <param name="comnum"���ں�></param>
        /// <param name="bodenum"������></param>
        /// <param name="datanum"����λ></param>
        /// <param name="stopnum"ֹͣλ></param>
        public static bool SetSP()
        {
            if (controlconfig.existItem("portnum")
                && controlconfig.existItem("bodenum")
                && controlconfig.existItem("datanum")
                && controlconfig.existItem("stopnum"))
            {
                if (controlconfig.valueItem("portnum") != null
                    && controlconfig.valueItem("bodenum") != null
                    && controlconfig.valueItem("datanum") != null
                    && controlconfig.valueItem("stopnum") != null)
                {
                    SetSP(controlconfig.valueItem("portnum"),
                        controlconfig.valueItem("bodenum"),
                        controlconfig.valueItem("datanum"),
                        controlconfig.valueItem("stopnum"));
                    return true;
                }  
            }
            return false;
        }
        public static bool SetSP(string comnum, int bodenum, int datanum, string stopnum)
        {
            if (sp.IsOpen)
            {
                sp.Close();
            }
            if (datanum > 8 || datanum < 5)
            {
                MessageBox.Show("����λ������ΧΪ[5,8]");
                return false;
            }
            sp.ReadBufferSize = 1;
            sp.PortName = comnum;
            sp.BaudRate = bodenum;
            sp.DataBits = datanum;
            switch (stopnum)
            {
                case "1":
                    sp.StopBits = StopBits.One;
                    break;
                case "2":
                    sp.StopBits = StopBits.Two;
                    break;
                case "1.5":
                    sp.StopBits = StopBits.OnePointFive;
                    break;
                default:
                    MessageBox.Show("ֹͣλ��������");
                    break;
            }
            //sp.Encoding = UTF8Encoding.UTF8;
            return true;
        }
        public static bool SetSP(string comnum, string bodenum, string datanum, string stopnum)
        {
            if (sp.IsOpen)
            {
                sp.Close();
            }
            int bodenumInt, datanumInt;
            if (!int.TryParse(bodenum, out bodenumInt))
            {
                MessageBox.Show("�����ʲ�������");
                return false;
            }
            if (!int.TryParse(datanum, out datanumInt))
            {
                MessageBox.Show("����λ��������");
                return false;
            }
            if (datanumInt > 8 || datanumInt < 5)
            {
                MessageBox.Show("����λ������ΧΪ[5,8]");
                return false;
            }
            //sp.ReadBufferSize = 1;
            sp.PortName = comnum;
            sp.BaudRate = int.Parse(bodenum);
            sp.DataBits = int.Parse(datanum);
            switch (stopnum)
            {
                case "1":
                    sp.StopBits = StopBits.One;
                    break;
                case "2":
                    sp.StopBits = StopBits.Two;
                    break;
                case "1.5":
                    sp.StopBits = StopBits.OnePointFive;
                    break;
                default:
                    MessageBox.Show("ֹͣλ��������");
                    return false;
            }
            return true;
        }


        /// <summary>
        /// �򿪴���
        /// </summary>
        /// <param name="comnum"></param>
        /// <param name="bodenum"></param>
        /// <param name="datanum"></param>
        /// <param name="stopnum"></param>
        /// <returns></returns>
        public static bool OpenSP()
        {
            if (sp.IsOpen)
            {
                sp.Close();
            }
            else
            {
                try
                {
                    sp.Open();
                }
                catch (Exception)
                {
                    return false;
                    throw;
                }
            }
            return true;
        }


        /// <summary>
        /// �رմ���
        /// true���رճɹ�
        /// false���ر�ʧ��
        /// </summary>
        /// <returns></returns>
        public static bool CloseSP()
        {
            sp.Close();
            return !sp.IsOpen;
        }


        /// <summary>
        /// �жϴ��ڿ���״̬
        /// true����״̬
        /// false���ر�״̬
        /// </summary>
        /// <returns></returns>
        public static bool IsComOpen()
        {
            return sp.IsOpen;
        }


        /// <summary>
        /// ��ȡ�������ݣ��������㹻bfSize�ֽڶ�ȡһ��
        /// </summary>
        /// <returns></returns>
        public static bool ReadSP(ref byte[] readData, int bfSize)
        {
            if (sp.BytesToRead >= bfSize)
            {
                sp.Read(readData, 0, bfSize);
                //Console.WriteLine("����readExisting:" + sp.BytesToRead.ToString() + "\r\n" + readData);
                //sp.DiscardInBuffer();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ReadSP(ref byte[] readData, ref string time, int bfSize)
        {
            if (sp.BytesToRead >= bfSize)
            {
                time = DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString();
                sp.Read(readData, 0, bfSize);
                //Console.WriteLine("����readExisting:" + sp.BytesToRead.ToString() + "\r\n" + readData);
                //sp.DiscardInBuffer();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ���ʹ�������
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        public static bool SendSP(string sendData)
        {
            sp.Write(sendData);
            return true;
        }

        /// <summary>
        /// ������뻺����
        /// </summary>
        public static void ClearInBuffer()
        {
            sp.DiscardInBuffer();
        }
    }
}
