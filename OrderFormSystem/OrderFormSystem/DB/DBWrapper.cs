using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace OrderFormSystem
{
    class DBWrapper
    {
        private string m_filePath;
        private string m_tableName;

        private DBWrapper()
        {

        }

        public DBWrapper(string filePath, string tableName)
        {
            m_filePath = filePath;
            m_tableName = tableName;
        }

        public bool Create(List<TableElement> elements)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();

                    string valueText = String.Empty;
                    for (int i = 0; i < elements.Count - 1; i++)
                    {
                        valueText += "@" + elements[i].Element + ",";
                    }
                    valueText += "@" + elements[elements.Count - 1].Element;

                    cmd.CommandText = "insert into [" + m_tableName + "] values ("+ valueText +");";

                    foreach (TableElement te in elements)
                    {
                        cmd.Parameters.Add(new SQLiteParameter(te.Element, te.Value));
                    }

                    int ret = cmd.ExecuteNonQuery();
                    return ret == 1;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(List<TableElement> elements)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();

                    string valueText = String.Empty;
                    for (int i = 1; i < elements.Count - 1; i++)
                    {
                        valueText += elements[i].Element + "=@" + elements[i].Element + ",";
                    }
                    valueText += elements[elements.Count - 1].Element + "=@" + elements[elements.Count - 1].Element;

                    cmd.CommandText = "update [" + m_tableName + "] set "+ valueText + 
                        " where " + elements[0].Element + "=@" + elements[0].Element + ";";

                    foreach (TableElement e in elements)
                    {
                        cmd.Parameters.Add(new SQLiteParameter(e.Element, e.Value));
                    }

                    int ret = cmd.ExecuteNonQuery();
                    return ret == 1;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(TableElement te)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from [" + m_tableName + "] where "+ 
                        te.Element +" =@"+ te.Element +";";
                    cmd.Parameters.Add(new SQLiteParameter(te.Element, te.Value));

                    int ret = cmd.ExecuteNonQuery();
                    return ret == 1;
                }
            }
            catch (Exception)
            {
                return false;

            }
        }


        public List<TableElement> GetRecord(TableElement te)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [" + m_tableName + "] where "+
                        te.Element +"=@" +te.Element+ ";";
                    cmd.Parameters.Add(new SQLiteParameter(te.Element, te.Value));

                    

                    SQLiteDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        List<TableElement> elements = new List<TableElement>();
                        for (int i = 0; i<dr.FieldCount; i++)
                        {
                            string name = dr.GetName(i);
                            object v =  dr.GetValue(i);
                            if (v == DBNull.Value)
                            {
                                v = null;
                            }
                            elements.Add(new TableElement(name,v));
                        }
                        return elements;
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

        public List<List<TableElement>> GetAllRecords()
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
                    List<List<TableElement>> elementList = new List<List<TableElement>>();
                    while (dr.Read())
                    {
                        List<TableElement> elements = new List<TableElement>();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            string name = dr.GetName(i);
                            object v = dr.GetValue(i);
                            if(v == DBNull.Value)
                            {
                                v = null;
                            }
                            elements.Add(new TableElement(name, v));
                        }
                        elementList.Add(elements);
                    }
                    return elementList;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool ClearRecord()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from [" + m_tableName + "];";
                    int ret = cmd.ExecuteNonQuery();
                    return ret != 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ClearRecord(TableElement te)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + m_filePath))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from [" + m_tableName + "] where "+ te.Element +" =@"+ te.Element +";";
                    cmd.Parameters.Add(new SQLiteParameter(te.Element, te.Value));

                    int ret = cmd.ExecuteNonQuery();
                    return ret != 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
