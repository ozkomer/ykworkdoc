using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OrderFormSystem
{
    public partial class UserForm : Form
    {
        private string m_userName;
        private string m_passwd;

        public string UserName
        {
            get { return m_userName; }
        }
        public string PassWord
        {
            get { return m_passwd; }
        }

        public UserForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_userName = this.textBoxUserName.Text;
            m_passwd = this.textBoxPasswd.Text;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
