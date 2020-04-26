using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace �𶯼��ϵͳ
{
    class DrawWave
    {
        public Point boxSize;
        public Bitmap bp;
        public static object wavelock = new object();

        public DrawWave(ref System.Windows.Forms.PictureBox box)
        {
            GetBoxSize(ref box);
        }

        public void GetBoxSize(ref System.Windows.Forms.PictureBox box)
        {
            boxSize.X = box.Width;
            boxSize.Y = box.Height;

            bp = new Bitmap(box.Width, box.Height);
        }

        /// <summary>
        /// ����X����̶��ߵķ���
        /// </summary>
        /// <param name="box">������Ƶ�pictureBox����</param>
        /// <param name="axisType">�����ͣ�0 ֱ��+�����꣬1 ֱ��+������+�����꣬2 �����꣬3 ������+������</param>
        /// <param name="axisMark">��̶�</param>
        public void DrawXaxis(ref System.Windows.Forms.PictureBox box, int axisType, int axisMark)
        {
            Graphics g;
            g = Graphics.FromImage(bp);//��������
            
            Pen py1 = new Pen(Color.Yellow, 1);//���廭��

            Point axisStart = new Point();//���������
            axisStart.X = 20;
            axisStart.Y = boxSize.Y - 20;

            int axisLenth = boxSize.X - 40;//�᳤��

            //��������
            for (int i = 0; i < axisLenth; i+=axisMark*5)
            {
                g.DrawLine(py1, axisStart.X + i, axisStart.Y - 4, axisStart.X + i, axisStart.Y + 4);
            }

            if (axisType == 1 || axisType == 3)
            {
                //��������
                for (int i = 0; i < axisLenth; i += axisMark)
                {
                    g.DrawLine(py1, axisStart.X + i, axisStart.Y - 2, axisStart.X + i, axisStart.Y + 2);
                }
            }

            if (axisType == 0 || axisType == 1)
            {
                //��ֱ��
                g.DrawLine(py1, axisStart.X, axisStart.Y, axisStart.X + axisLenth, axisStart.Y);
            }

            box.Image = bp;

            g.Dispose();
            py1.Dispose();
        }

        /// <summary>
        /// ����Y����̶��ߵķ���
        /// </summary>
        /// <param name="box">������Ƶ�pictureBox����</param>
        /// <param name="axisType">�����ͣ�0 ֱ��+�����꣬1 ֱ��+������+�����꣬2 �����꣬3 ������+������</param>
        /// <param name="axisMark">��̶�</param>
        public void DrawYaxis(ref System.Windows.Forms.PictureBox box, int axisType, int axisMark)
        {
            Graphics g;
            g = Graphics.FromImage(bp);//��������
            Pen py1 = new Pen(Color.Yellow, 1);

            Point axisStart = new Point();//���������
            axisStart.X = 20;
            axisStart.Y = boxSize.Y - 20;

            int axisLenth = boxSize.Y - 40;//�᳤��

            //��������
            for (int i = 0; i < axisLenth; i += axisMark*5)
            {
                g.DrawLine(py1, axisStart.X - 4, axisStart.Y - i, axisStart.X + 4, axisStart.Y - i);
            }

            if (axisType == 1 || axisType == 3)
            {
                //��������
                for (int i = 0; i < axisLenth; i += axisMark)
                {
                    g.DrawLine(py1, axisStart.X - 2, axisStart.Y - i, axisStart.X + 2, axisStart.Y - i);
                }
            }

            if (axisType == 0 || axisType == 1)
            {
                //��ֱ��
                g.DrawLine(py1, axisStart.X, axisStart.Y, axisStart.X, axisStart.Y - axisLenth);
            }

            box.Image = bp;

            g.Dispose();
            py1.Dispose();
        }

        /// <summary>
        /// ���Ʋ��εķ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="box">��Ҫ���Ƶ�ͼƬ����</param>
        /// <param name="Xmark">ʱ������������</param>
        /// <param name="timeType">ʱ������</param>
        public void Wave(ref UInt16[] data, ref System.Windows.Forms.PictureBox box, int Xmark, int timeType)
        {
            Graphics g;
            g = Graphics.FromImage(bp);//��������
            Pen pwave = new Pen(Color.YellowGreen, 1);

            int num = data.Length;
            //UInt16 max = data.Max(), min = data.Min();
            Point startPoint = new Point(20, boxSize.Y - 20);
            Point maxPoint = new Point(boxSize.X - 20, 20);

            int xPointNum = (boxSize.X - 40) / 5 + 1;

            ClearToBackcolor(ref box); 
            DrawXaxis(ref box, 1, 5);
            DrawYaxis(ref box, 1, 5);
            if (num <= xPointNum)   //data num <= x axis point num
            {
                for (int i = 0; i < num-1; i++)
                {
                    float yLess1 = (float)data[i] / 65535 * (boxSize.Y - 40);
                    float yLess2 = (float)data[i+1] / 65535 * (boxSize.Y - 40);
                    g.DrawLine(pwave, startPoint.X + i * 5, startPoint.Y - yLess1, startPoint.X + (i + 1) * 5, startPoint.Y - yLess2);
                }
            }
            else    //data num > x axis point num
            {
                for (int i = num - xPointNum; i < num-1; i++)
                {
                    float yLess1 = (float)data[i] / 65535 * (boxSize.Y - 40);
                    float yLess2 = (float)data[i + 1] / 65535 * (boxSize.Y - 40);
                    g.DrawLine(pwave, startPoint.X + i * 5, startPoint.Y - yLess1, startPoint.X + (i + 1) * 5, startPoint.Y - yLess2);
                }
            }

            box.Image = bp;

            g.Dispose();
            pwave.Dispose();
        }
        public void Wave(ref UInt16 data, ref System.Windows.Forms.PictureBox box, int Xmark, int timeType)
        {
            Graphics g;
            g = Graphics.FromImage(bp);//��������
            Pen pwave = new Pen(Color.YellowGreen, 1);

            Point startPoint = new Point(20, boxSize.Y - 20);
            Point maxPoint = new Point(boxSize.X - 20, 20);

            int xPointNum = (boxSize.X - 40) / 5 + 1;

            ClearToBackcolor(ref box);
            DrawXaxis(ref box, 1, 5);
            DrawYaxis(ref box, 1, 5);

            box.Image = bp;

            g.Dispose();
            pwave.Dispose();
        }

        /// <summary>
        /// Clear bitmap to box.ackColor
        /// </summary>
        /// <param name="box"></param>
        public void ClearToBackcolor(ref System.Windows.Forms.PictureBox box)
        {
            Graphics g;
            g = Graphics.FromImage(bp);//��������

            g.Clear(box.BackColor);

            g.Dispose();
        }
    }
}
