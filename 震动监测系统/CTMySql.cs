using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace �𶯼��ϵͳ
{
    public class CTMySql
    {
        static public bool isSignIn = false;//�û��Ƿ��¼��־
        static public bool isUserAdmin = false;//�û��Ƿ�Ϊ����Ա�˻���־

        static MySqlConnection conn = new MySqlConnection();
        static MySqlCommand cmd = new MySqlCommand();
        static MySqlCommandBuilder cmdb = new MySqlCommandBuilder();
        static MySqlDataAdapter dtadp = new MySqlDataAdapter();
        static public DataSet dtst = new DataSet("�𶯼��ϵͳ");//�����ڴ��
        public DataTable dttb;

        /// <summary>
        /// ���캯�����������ݿ����
        /// </summary>
        /// <param name="server"></param>
        /// <param name="user"></param>
        /// <param name="databass"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        public CTMySql() { }
        public CTMySql(string server, string user, string databass, string password, string port)
        {
            //Server = server;
            //User = user;
            //Databass = databass;
            //Password = password;
            //Port = port;

            conn.ConnectionString = string.Format("server={0};port={1};uid={2};pwd={3};database={4}"
                , server, port, user, password, databass);
        }

        /// <summary>
        /// �������ݿ�
        /// </summary>
        /// <returns></returns>
        public bool ConnectDatabass()
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// �ж��û���¼�����Ƿ���ȷ
        /// </summary>
        /// <param name="account">�û���</param>
        /// <param name="password">����</param>
        /// <returns></returns>
        public bool CheckSignInPassword(string account, string password)
        {
            cmd.CommandText = "account";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.TableDirect;
            //conn = new MySqlConnection();
            //conn.ConnectionString = string.Format("server=localhost;port=3306;uid=root;pwd=000000;database=�𶯼��ϵͳ");
            ConnectDatabass();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3}", reader[0], reader[1], reader[2], reader[3]));
                if (reader[1].ToString() == password && reader[2].ToString() == account)//�ж��û����������Ƿ���ȷ
                {
                    isSignIn = true;
                    if (reader[3].ToString() == "1")//�ж��û��Ƿ�Ϊ����Ա�û�
                    {
                        isUserAdmin = true;
                    }
                    else
                    {
                        isUserAdmin = false;
                    }
                    reader.Close();
                    CloseConnect();
                    return true;
                }
            }
            reader.Close();
            reader.Dispose();
            CloseConnect();
            return false;
        }

        /// <summary>
        /// �Ͽ����ݿ�����
        /// </summary>
        /// <returns></returns>
        static public bool CloseConnect()
        {
            if (conn.State == ConnectionState.Open || conn.State == ConnectionState.Connecting)
            {
                conn.Dispose();
                return true;
            }
            return false;
        }

        /// <summary>
        /// ��ȡ���б���
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDatatable()
        {
            ConnectDatabass();
            List<string> s = new List<string>(); 
            string sql = "show tables;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                string t = reader.GetString(0);
                s.Add(t);
            }
            reader.Close();
            CloseConnect();
            return s;
        }

        /// <summary>
        /// �����±��������
        /// </summary>
        /// <param name="tablename">����</param>
        public void CreateNewTable(string tablename)
        {
            cmd.CommandText = string.Format("CREATE TABLE `�𶯼��ϵͳ`.`{0}` (" +
                "`DataID` INT NOT NULL, " +
                "`DataTime` CHAR(23) NOT NULL, " +
                "`DataValue` SMALLINT UNSIGNED NOT NULL, " +
                "PRIMARY KEY(`DataID`)); ", tablename);
            Console.WriteLine(cmd.CommandText);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                //conn.Dispose();
            }
            catch (Exception)
            {

            }

            cmd.ExecuteNonQuery();
            Console.WriteLine("new table has create!!");
            Console.WriteLine(cmd.CommandText);
            Console.WriteLine();
        }

        /// <summary>
        /// �½��ڴ��
        /// </summary>
        /// <param name="tablename">����</param>
        public void CreateDSTable(string tablename)
        {
            dttb = new DataTable(tablename);//�����ڴ��
            dtst.Tables.Add(dttb);//���ڴ����ӵ��ڴ��

            //�����ڴ���
            DataColumn dtclID = new DataColumn("DataID", typeof(int));
            DataColumn dtclTime = new DataColumn("DataTime", typeof(string));
            DataColumn dtclValue = new DataColumn("DataValue", typeof(UInt16));

            dttb.Columns.AddRange(new DataColumn[] { dtclID, dtclTime, dtclValue });//���ڴ�����ӵ��ڴ��
        }

        /// <summary>
        /// ���ڴ�����һ������
        /// </summary>
        /// <param name="tablename">strin ����</param>
        /// <param name="dataid">uint32 ID</param>
        /// <param name="datatime">string ʱ��</param>
        /// <param name="datavalue">uint16 ��ֵ</param>
        public void InsertData2DSTable(string tablename, int dataid, string datatime, ref UInt16[] datavalue)
        {
            dttb = dtst.Tables[tablename];
            for(int i = 0; i < datavalue.Length; i++)
                dttb.Rows.Add(dataid++, datatime, datavalue[i]);
        }

        /// <summary>
        /// ���ڴ��ĩβ100�����ӡ����µ����ݱ�
        /// </summary>
        /// <param name="tablename">string ����</param>
        public void AddOrUpdataTableFromDataset2Databass(string tablename)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO `" + tablename + "` (");
            for (int i = 0; i < dtst.Tables[tablename].Columns.Count; i++)
            {
                sb.Append(dtst.Tables[tablename].Columns[i].ColumnName + ",");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append(") VALUES ");
            for (int i = dtst.Tables[tablename].Rows.Count - 100; i < dtst.Tables[tablename].Rows.Count; i++)
            {
                sb.Append("(");
                for (int j = 0; j < dtst.Tables[tablename].Columns.Count; j++)
                {
                    sb.Append("'" + dtst.Tables[tablename].Rows[i][j] + "',");
                }
                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("),");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append(";");

            conn.Open();
            cmd.CommandText = sb.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            sb.Clear();
            //conn.Dispose();
        }
        /// <summary>
        /// ��ָ��λ�ú��������ڴ�����ӡ����µ����ݱ�
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="num"></param>
        /// <param name="start"></param>
        public void AddOrUpdataTableFromDataset2Databass(string tablename, int num, int start)
        {
            MySqlCommand cmd1 = new MySqlCommand();
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO `" + tablename + "` (");
            for (int i = 0; i < dtst.Tables[tablename].Columns.Count; i++)
            {
                sb.Append(dtst.Tables[tablename].Columns[i].ColumnName + ",");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append(") VALUES ");
            for (int i = 0; i < num; i++)
            {
                sb.Append("(");
                for (int j = 0; j < dtst.Tables[tablename].Columns.Count; j++)
                {
                    sb.Append("'" + dtst.Tables[tablename].Rows[start + i][j] + "',");
                }
                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("),");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append(";");

            cmd1.CommandText = sb.ToString();
            cmd1.CommandType = CommandType.Text;
            cmd1.Connection = conn;
            //bool a = ConnectDatabass();
            //Console.WriteLine(sb);
            if (conn.State == ConnectionState.Closed)
                ConnectDatabass();
            //try
            //{
            //    conn.Open();
            //}
            //catch (Exception)
            //{
            //}
            cmd1.ExecuteNonQuery();
            cmd1.Dispose();
            //sb.Clear();
            //conn.Dispose();
        }

        /// <summary>
        /// ����ڴ��
        /// </summary>
        public void ClearDataSet()
        {
            dtst.Tables[0].Clear();
            dtst.Tables[1].Clear();
            dtst.Tables[0].Dispose();
            dtst.Tables[1].Dispose();
            dtst.Clear();
            //dtst.Dispose();
        }

        /// <summary>
        /// ���ָ���ڴ��
        /// </summary>
        /// <param name="tablename"></param>
        public void ClearDataTable(string tablename)
        {
            dtst.Tables[tablename].Dispose();
        }

        /// <summary>
        /// ��ȡָ���ڴ������
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public UInt32 NumOfDSTableRow(string tablename)
        {
            UInt32 num = (UInt32)dtst.Tables[tablename].Rows.Count;
            return num;
        }
        public UInt32 NumOfDSTableRow(DataTable datatable)
        {
            UInt32 num = (UInt32)datatable.Rows.Count;
            return num;
        }

        /// <summary>
        /// ��ȡ���ڴ���ֽ���
        /// </summary>
        /// <returns></returns>
        public UInt32 SizeOfDataSet()
        {
            UInt32 a1 = NumOfDSTableRow(dtst.Tables[0]);
            return a1 * 227;
        }

        /// <summary>
        /// ����̨��ӡ�ڴ���ܴ�С
        /// </summary>
        public void PrintSizeOfDataSet()
        {
            UInt32 MB = SizeOfDataSet() / (1024 * 1024);
            UInt32 KB = SizeOfDataSet() % (1024 * 1024) / 1024;
            Console.WriteLine("DataSet Size: {0} MB {1} KB", MB, KB);
        }
    }
}
