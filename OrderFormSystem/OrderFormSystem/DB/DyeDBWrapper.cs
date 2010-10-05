using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace OrderFormSystem
{
    class DyeDBWrapper
    {
        private static string m_filePath = @".\user.db3";
        private static string m_tableName = @"Dye";

        public static bool Create(Dye dye)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into [" + m_tableName + "] values (@DyeCode,@DyeType,@DyeName,@DyeValve);";
                    cmd.Parameters.Add(new SQLiteParameter("DyeCode", dye.DyeCode));
                    cmd.Parameters.Add(new SQLiteParameter("DyeType", dye.DyeType));
                    cmd.Parameters.Add(new SQLiteParameter("DyeName", dye.DyeName));
                    cmd.Parameters.Add(new SQLiteParameter("DyeValve", dye.DyeValve));

                    int i = cmd.ExecuteNonQuery();
                    return i == 1;
                }
            }
            catch (Exception)
            {
                return false;

            }
        }

        public static bool Update(Dye dye)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "update [" + m_tableName + "] set DyeType=@DyeType, DyeName=@DyeName, DyeValve=@DyeValve where DyeCode=@DyeCode;";
                    cmd.Parameters.Add(new SQLiteParameter("DyeCode", dye.DyeCode));
                    cmd.Parameters.Add(new SQLiteParameter("DyeType", dye.DyeType));
                    cmd.Parameters.Add(new SQLiteParameter("DyeName", dye.DyeName));
                    cmd.Parameters.Add(new SQLiteParameter("DyeValve", dye.DyeValve));
                    

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

        public static bool Delete(string dyCode)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from [" + m_tableName + "] where DyeCode=@DyeCode;";
                    cmd.Parameters.Add(new SQLiteParameter("DyeCode", dyCode));

                    int i = cmd.ExecuteNonQuery();
                    return i == 1;
                }
            }
            catch (Exception)
            {
                return false;

            }
        }

        public static Dye GetbyName(string dyeCode)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [" + m_tableName + "] where DyeCode=@DyeCode;";
                    cmd.Parameters.Add(new SQLiteParameter("DyeCode", dyeCode));

                    SQLiteDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        Dye d = new Dye(dr.GetString(0), (DyeType)dr.GetInt32(1), dr.GetString(2), dr.GetInt32(3));
                        return d;
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

        public static List<string> GetAllCodes()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [" + m_tableName + "];";

                    SQLiteDataReader dr = cmd.ExecuteReader();
                    List<string> dyeCodes = new List<string>();
                    while (dr.Read())
                    {
                        Dye d = new Dye(dr.GetString(0), (DyeType)dr.GetInt32(1), dr.GetString(2), dr.GetInt32(3));
                        dyeCodes.Add(d.DyeCode);
                    }

                    return dyeCodes;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Dye> GetAllDyes(DyeType dt)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [" + m_tableName + "] where DyeType = @DyeType;";
                    cmd.Parameters.Add(new SQLiteParameter("DyeType", dt));

                    SQLiteDataReader dr = cmd.ExecuteReader();
                    List<Dye> dyes = new List<Dye>();
                    while (dr.Read())
                    {
                        Dye d = new Dye(dr.GetString(0), (DyeType)dr.GetInt32(1), dr.GetString(2), dr.GetInt32(3));
                        dyes.Add(d);
                    }

                    return dyes;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<string> GetNamesByType(DyeType dt)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    int intUT = (int)dt;
                    cmd.CommandText = "SELECT * FROM [" + m_tableName + "] where DyeType=" + intUT.ToString() + ";";

                    SQLiteDataReader dr = cmd.ExecuteReader();
                    List<string> dyeCodes = new List<string>();
                    while (dr.Read())
                    {
                        Dye d = new Dye(dr.GetString(0), (DyeType)dr.GetInt32(1), dr.GetString(2), dr.GetInt32(3));
                        dyeCodes.Add(d.DyeCode);
                    }

                    return dyeCodes;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool DeleteAll(DyeType dt)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from [" + m_tableName + "] where DyeType = @DyeType;";
                    cmd.Parameters.Add(new SQLiteParameter("DyeType", dt));
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
