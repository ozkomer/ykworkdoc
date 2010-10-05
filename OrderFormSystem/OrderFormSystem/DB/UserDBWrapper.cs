using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace OrderFormSystem
{
    class UserDBWrapper
    {
        //private static string m_filePath = @"C:\Users\fanca\Documents\Visual Studio 2010\Projects\OrderFormSystem\user.db3";
        private static string m_filePath = @".\user.db3";
        private static string m_tableName = @"User";

        public static bool Create(User user)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into [" + m_tableName + "] values (@UserName,@PassWord,@UserType);";
                    cmd.Parameters.Add(new SQLiteParameter("UserName", user.Name));
                    cmd.Parameters.Add(new SQLiteParameter("PassWord", user.PassWord));
                    cmd.Parameters.Add(new SQLiteParameter("UserType", user.UserType));

                    int i = cmd.ExecuteNonQuery();
                    return i == 1;
                }
            }
            catch (Exception)
            {
                return false;

            }
        }

        public static bool Update(User user)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "update [" + m_tableName + "] set UserType=@UserType, PassWord=@PassWord where UserName=@UserName;";
                    cmd.Parameters.Add(new SQLiteParameter("UserName", user.Name));
                    cmd.Parameters.Add(new SQLiteParameter("PassWord", user.PassWord));
                    cmd.Parameters.Add(new SQLiteParameter("UserType", user.UserType));

                    int i = cmd.ExecuteNonQuery();

                    return i == 1;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Delete(User user)
        {
            return Delete(user.Name);
        }

        public static bool Delete(string name)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from [" + m_tableName + "] where UserName=@UserName;";
                    cmd.Parameters.Add(new SQLiteParameter("UserName", name));

                    int i = cmd.ExecuteNonQuery();
                    return i == 1;
                }
            }
            catch (Exception)
            {
                return false;

            }
        }

        public static User GetbyName(string name)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [" + m_tableName + "] where UserName=@UserName;";
                    cmd.Parameters.Add(new SQLiteParameter("UserName", name));

                    SQLiteDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        User user = new User(dr.GetString(0), dr.GetString(1), (UserType)dr.GetInt32(2));
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<string> GetAllNames()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [" + m_tableName + "];";

                    SQLiteDataReader dr = cmd.ExecuteReader();
                    List<string> names = new List<string>();
                    while (dr.Read())
                    {
                        User user = new User(dr.GetString(0), dr.GetString(1), (UserType)dr.GetInt32(2));
                        names.Add(user.Name);
                    }

                    return names;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<string> GetNamesByType(UserType ut)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    int intUT = (int)ut;
                    cmd.CommandText = "SELECT * FROM [" + m_tableName + "] where UserType=" + intUT.ToString() + ";";

                    SQLiteDataReader dr = cmd.ExecuteReader();
                    List<string> names = new List<string>();
                    while (dr.Read())
                    {
                        User user = new User(dr.GetString(0), dr.GetString(1), (UserType)dr.GetInt32(2));
                        names.Add(user.Name);
                    }

                    return names;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool DeleteAll()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from [" + m_tableName + "];";
                    int i = cmd.ExecuteNonQuery();
                    return i == 1;
                }
            }
            catch (Exception)
            {
                return false;

            }
        }
    }
}
