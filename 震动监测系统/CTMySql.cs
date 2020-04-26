﻿using System;
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

namespace 震动监测系统
{
    public class CTMySql
    {
        static public bool isSignIn = false;//用户是否登录标志
        static public bool isUserAdmin = false;//用户是否为管理员账户标志

        static MySqlConnection conn;
        static MySqlCommand cmd = new MySqlCommand();
        static MySqlCommandBuilder cmdb = new MySqlCommandBuilder();
        static MySqlDataAdapter dtadp = new MySqlDataAdapter();
        static public DataSet dtst = new DataSet("震动监测系统");//创建内存库
        public DataTable dttb;

        /// <summary>
        /// 构造函数，设置数据库参数
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

            conn = new MySqlConnection();
            conn.ConnectionString = string.Format("server={0};port={1};uid={2};pwd={3};database={4}"
                , server, port, user, password, databass);
        }

        /// <summary>
        /// 连接数据库
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
        /// 判断用户登录密码是否正确
        /// </summary>
        /// <param name="account">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool CheckSignInPassword(string account, string password)
        {
            cmd.CommandText = "account";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.TableDirect;
            ConnectDatabass();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3}", reader[0], reader[1], reader[2], reader[3]));
                if (reader[1].ToString() == password && reader[2].ToString() == account)//判断用户名和密码是否正确
                {
                    isSignIn = true;
                    if (reader[3].ToString() == "1")//判断用户是否为管理员用户
                    {
                        isUserAdmin = true;
                    }
                    else
                    {
                        isUserAdmin = false;
                    }
                    reader.Close();
                    conn.Dispose();
                    return true;
                }
            }
            reader.Close();
            conn.Dispose();
            return false;
        }

        /// <summary>
        /// 断开数据库连接
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
        /// 创建新表，传入表名
        /// </summary>
        /// <param name="tablename">表名</param>
        public void CreateNewTable(string tablename)
        {
            cmd.CommandText = string.Format("CREATE TABLE `震动监测系统`.`{0}` (" +
                "`DataID` INT NOT NULL, " +
                "`DataTime` CHAR(23) NOT NULL, " +
                "`DataValue` UNSIGNED SMALLINT NOT NULL, " +
                "PRIMARY KEY(`DataID`)); ", tablename);
            Console.WriteLine(cmd.CommandText);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Dispose();

            Console.WriteLine("new table has create!!");
            Console.WriteLine(cmd.CommandText);
            Console.WriteLine();
        }

        /// <summary>
        /// 新建内存表
        /// </summary>
        /// <param name="tablename">表名</param>
        public void CreateDSTable(string tablename)
        {
            dttb = new DataTable(tablename);//创建内存表
            dtst.Tables.Add(dttb);//把内存表添加到内存库

            //创建内存列
            DataColumn dtclID = new DataColumn("DataID", typeof(int));
            DataColumn dtclTime = new DataColumn("DataTime", typeof(string));
            DataColumn dtclValue = new DataColumn("DataValue", typeof(UInt16));

            dttb.Columns.AddRange(new DataColumn[] { dtclID, dtclTime, dtclValue });//把内存列添加到内存表
        }

        /// <summary>
        /// 给内存表插入一行数据
        /// </summary>
        /// <param name="tablename">strin 表名</param>
        /// <param name="dataid">uint32 ID</param>
        /// <param name="datatime">string 时间</param>
        /// <param name="datavalue">uint16 数值</param>
        public void InsertData2DSTable(string tablename, int dataid, string datatime, ref UInt16[] datavalue)
        {
            dttb = dtst.Tables[tablename];
            for(int i = 0; i < datavalue.Length; i++)
                dttb.Rows.Add(dataid, datatime, datavalue[i]);
        }

        /// <summary>
        /// 从内存表增加、更新数据表
        /// </summary>
        /// <param name="tablename">string 表名</param>
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
            conn.Dispose();
        }

        /// <summary>
        /// 清空内存库
        /// </summary>
        public void ClearDataSet()
        {
            dtst.Dispose();
        }

        /// <summary>
        /// 清空指定内存表
        /// </summary>
        /// <param name="tablename"></param>
        public void ClearDataTable(string tablename)
        {
            dtst.Tables[tablename].Dispose();
        }

        /// <summary>
        /// 获取指定内存表行数
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
        /// 获取总内存库字节数
        /// </summary>
        /// <returns></returns>
        public UInt32 SizeOfDataSet()
        {
            UInt32 a1 = NumOfDSTableRow(dtst.Tables[0]);
            return a1 * 227;
        }

        /// <summary>
        /// 控制台打印内存库总大小
        /// </summary>
        public void PrintSizeOfDataSet()
        {
            UInt32 MB = SizeOfDataSet() / (1024 * 1024);
            UInt32 KB = SizeOfDataSet() % (1024 * 1024) / 1024;
            Console.WriteLine("DataSet Size: {0} MB {1} KB", MB, KB);
        }
    }
}
