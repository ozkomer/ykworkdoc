using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace OrderFormSystem.DB
{
    class DyeDB
    {
        private static string m_filePath = @".\user.db3";
        private static string m_tableName = @"Dye";
        private static DBWrapper m_db = new DBWrapper(m_filePath,m_tableName);

        public static bool Create(Dye dye)
        {
            return m_db.Create(dye.Elements);
        }

        public static bool Update(Dye dye)
        {
            return m_db.Update(dye.Elements);
        }

        public static bool Delete(string dyeCode)
        {
            TableElement te = new TableElement("DyeCode", dyeCode);
            return m_db.Delete(te);
        }

        public static Dye GetRecord(string dyeCode)
        {
            TableElement te = new TableElement("DyeCode", dyeCode);
            List<TableElement> elements = m_db.GetRecord(te);
            if (elements == null)
            {
                return null;
            }
            Dye dye = new Dye(elements);
            return dye;
        }

        public static List<Dye> GetAllRecords()
        {
            List<List<TableElement>> le = m_db.GetAllRecords();
            List<Dye> dyes = new List<Dye>();
            foreach (List<TableElement> elements in le)
            {
                dyes.Add(new Dye(elements));
            }
            return dyes;
        }

        public static List<Dye> GetAllRecords(DyeType dt)
        {
            List<Dye> dyes= GetAllRecords();
            List<Dye> newDyes = new List<Dye>();

            foreach (Dye d in dyes)
            {
                if (d.DyeType == dt)
                {
                    newDyes.Add(d);
                }
            }
            
            return newDyes;
        }

        public static bool DeleteAll(DyeType dt)
        {
            TableElement te = new TableElement("DyeType", dt);
            return m_db.ClearRecord(te);
        }

        public static bool ClearAllRecords()
        {
            return m_db.ClearRecord();
        }
    }
}
