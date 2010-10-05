using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFormSystem.DB
{
    class OrderDB
    {
        private static string m_filePath = @".\user.db3";
        private static string m_tableName = @"Order";

        private static DBWrapper m_db = new DBWrapper(m_filePath, m_tableName);

        public static bool Create(Order od)
        {
            return m_db.Create(od.Elements);
        }

        public static bool Update(Order od)
        {
            return m_db.Update(od.Elements);
        }

        public static bool Delete(string orderID)
        {
            TableElement te = new TableElement("OrderID", orderID);
            return m_db.Delete(te);
        }

        public static Order GetRecord(string orderID)
        {
            TableElement te = new TableElement("OrderID", orderID);
            List<TableElement> elements = m_db.GetRecord(te);
            Order od = new Order(elements);
            return od;
        }

        public static List<Order> GetAllRecords()
        {
            List<List<TableElement>> le = m_db.GetAllRecords();
            List<Order> orders = new List<Order>();
            foreach (List<TableElement> elements in le)
            {
                orders.Add(new Order(elements));
            }
            return orders;
        }

        public static bool ClearAllRecords()
        {
            return m_db.ClearRecord();
        }
    }
}
