using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFormSystem.DB
{
    class UserDB
    {
        private static string m_filePath = @".\user.db3";
        private static string m_tableName = @"User";
        private static DBWrapper m_db = new DBWrapper(m_filePath, m_tableName);

        public static bool Create(User user)
        {
            return m_db.Create(user.Elements);
        }

        public static bool Update(User u)
        {
            return m_db.Update(u.Elements);
        }

        public static bool Delete(string userName)
        {
            TableElement te = new TableElement("UserName", userName);
            return m_db.Delete(te);
        }

        public static User GetRecord(string userName)
        {
            TableElement te = new TableElement("UserName", userName);
            List<TableElement> elements = m_db.GetRecord(te);
            User u = new User(elements);
            return u;
        }

        public static List<User> GetAllRecords()
        {
            List<List<TableElement>> le = m_db.GetAllRecords();
            List<User> users = new List<User>();
            foreach (List<TableElement> elements in le)
            {
                users.Add(new User(elements));
            }
            return users;
        }

        public static List<User> GetAllRecords(UserType ut)
        {
            List<User> users = GetAllRecords();
            List<User> newUsers = new List<User>();
            foreach (User u in users)
            {
                if (u.UserType == ut)
                {
                    newUsers.Add(u);
                }
            }
            return newUsers;
        }

        public static bool ClearAllRecords()
        {
            return m_db.ClearRecord();
        }
    }
}
