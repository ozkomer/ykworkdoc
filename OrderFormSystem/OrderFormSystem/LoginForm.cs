using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OrderFormSystem.Properties;
using OrderFormSystem.DB;

namespace OrderFormSystem
{
    public partial class LoginForm : Form
    {
        //private User m_user = UserDBWrapper.GetbyName("admin");
        private User m_user = UserDB.GetRecord("admin");
        private bool m_loginSucces = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        #region public properties
        public User User
        {
            get { return m_user; }
        }
        public UserType UserType
        {
            get { return m_user.UserType; }
        }
        public bool Success
        {
            get { return m_loginSucces; }
        }
        #endregion

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string userName = this.textBoxName.Text;
            string passwd = this.textBoxPasswd.Text;
            UserType ut = UserType.SuperUser;

            m_loginSucces = false;

            //m_user = UserDBWrapper.GetbyName(userName);
            m_user = UserDB.GetRecord(userName);
            if (m_user != null)
            {
                if (m_user.PassWord == passwd)
                {
                    ut = m_user.UserType;
                    m_loginSucces = true;
                }
                else
                {
                    MessageBox.Show(Resources.LoginPasswordError);
                }
                
            }
            else
            {
                MessageBox.Show(Resources.LoginNoName);
            }
       }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
