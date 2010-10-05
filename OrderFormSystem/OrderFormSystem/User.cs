using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFormSystem
{
    public enum UserType
    {
        SuperUser = 0,
        ManageUser = 1,
        NormalUser = 2
    }

    public class User
    {
        #region private vars
        private string m_name = null;
        private string m_passwd = null;
        private UserType m_type = UserType.NormalUser;
        private List<TableElement> m_elements;
        #endregion
        
        #region public constructor
        public User(string name, string passwd, UserType ut)
        {
            m_name = name;
            m_passwd = passwd;
            m_type = ut;

            TableElement t1 = new TableElement("UserName", name);
            TableElement t2 = new TableElement("PassWord", passwd);
            TableElement t3 = new TableElement("UserType", ut);
            m_elements = new List<TableElement>();
            m_elements.Add(t1);
            m_elements.Add(t2);
            m_elements.Add(t3);
        }

        public User(List<TableElement> elements)
        {
            m_name = (string)elements[0].Value;
            m_passwd = (string)elements[1].Value;
            m_type = (UserType)(Convert.ToInt32(elements[2].Value));

            m_elements = elements;
        }
        #endregion
        

        #region public properties
        public List<TableElement> Elements
        {
            get { return m_elements; }
        }

        public string Name
        {
            get { return m_name; }
        }
        
        public string PassWord
        {
            get { return m_passwd; }
            set 
            {
                m_passwd = value;
                m_elements[2].Value = value;
            }
        }

        public UserType UserType
        {
            get { return m_type;}
        }
        #endregion

        #region public methods
        public bool Verify()
        {
            //TODO
            return true;
        }
        #endregion
    }
}
