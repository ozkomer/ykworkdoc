using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OrderFormSystem.Properties;

namespace OrderFormSystem
{
    public partial class RegisterForm : Form
    {
        public enum RFState
        {
            FirstTime = 0,
            Tips = 1,
            Invalid = 2
        };

        public RegisterForm(RFState rfs)
        {
            InitializeComponent();
            switch(rfs)
            {
                case RFState.FirstTime:
                    break;
                case RFState.Tips:
                    this.labelTips.Text = "软件即将过期,请注册,以方便您的继续使用!";
                    break;
                case RFState.Invalid:
                    this.labelTips.Text = "软件试用期已过,请注册后再使用!";
                    this.buttonTry.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string registerCode = Register.RegisterHelper.GetRegisterCode();
            if (this.textBoxRegisterCode.Text.Equals(registerCode))
            {
                Register.RegisterHelper.WTRegedit(Register.RegisterHelper.REGISTER_KEY,registerCode);
                MessageBox.Show(Resources.RegisterSuccess);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(Resources.RegisterFailed);
            }
        }

        private void buttonTry_Click(object sender, EventArgs e)
        {
            Register.RegisterHelper.WTRegedit(Register.RegisterHelper.TRY_DAY, DateTime.Now.ToString());
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            this.textBoxMachineCode.Text = Register.RegisterHelper.GetMachineCode();
        }
    }
}
