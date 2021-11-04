using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppAS228T.Common;
using WpfAppAS228T.DataAccess.DataEntity;

namespace WpfAppAS228T.DataAccess
{
    public class LocalDataAccess
    {
        private SQLiteConnection conn;
        private SQLiteCommand comm;
        private SQLiteDataAdapter adapter;
        private SQLiteHelper sh;

        public LocalDataAccess()
        {

            
        }

        private static LocalDataAccess instance;
        public static LocalDataAccess GetInstance()
        {
            return instance ?? (instance = new LocalDataAccess());
        }

        private bool DBConnection()
        {

            SQLiteConnectionStringBuilder connStr = new SQLiteConnectionStringBuilder();
            connStr["Data Source"] = $"{Environment.CurrentDirectory}" + "/AppDb.db";


            //string connStr = "data source = ../Assets/DataBase/AppDb.db";

            if (conn == null)
            {
                conn = new SQLiteConnection(connStr.ConnectionString);
            }
            if (comm == null)
            {
                comm = new SQLiteCommand();
                comm.Connection = conn;
            }
            try
            {

                conn.Open();
                this.sh = new SQLiteHelper(comm);
                return true;
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("连接数据库",ex);
                return false; ;
            }
        }

        public UserEntity CheckUserInfo(string userName, string pwd)
        {
            try
            {
                if (DBConnection())
                {
                    //string userSql = $"select * from UserTable where UserName='{userName}' and PassWord='{pwd}'";

                    string userSql = "select * from UserTable where UserName=@username and PassWord=@pwd;";

                    var dic = new Dictionary<string, object> { };
                    dic["@username"] = userName;
                    dic["@pwd"] = pwd;

                    DataTable table = sh.Select(userSql,dic);

                    if (table.Rows.Count == 0)
                    {
                        throw new Exception("用户名或密码不正确！");
                    }
                    DataRow dr = table.Rows[0]; //获取第一行

                    UserEntity userInfo = new UserEntity();
                    userInfo.UserName = dr.Field<string>("UserName");
                    userInfo.Password = dr.Field<string>("PassWord");
                    userInfo.Id = dr.Field<int>("Id");

                    return userInfo;

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog("数据库查询", ex);
            }
            finally
            {
                this.Dispose();
            }


            return null;
        }

        private void Dispose()
        {
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm != null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }
    }
}
