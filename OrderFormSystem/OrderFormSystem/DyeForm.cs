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
    public partial class DyeForm : Form
    {
        DyeType m_DyeType = DyeType.AUXILIARY;

        public DyeForm(DyeType dt)
        {
            InitializeComponent();

            switch(dt)
            {
                case DyeType.DYE:
                    this.Text = "染料设置";
                    this.labelCode.Text = "染料代号:";
                    this.labelName.Text = "染料名称:";

                    this.labelValve.Hide();
                    this.textBoxValve.Hide();

                    m_DyeType = DyeType.DYE;
                    break;
                case DyeType.AUXILIARY:
                    this.Text = "助剂设置";
                    this.labelCode.Text = "助剂代号:";
                    this.labelName.Text = "助剂名称:";


                    this.labelValve.Show();
                    this.textBoxValve.Show();

                    m_DyeType = DyeType.AUXILIARY;
                    break;
                default:
                    break;
            }
        }

        #region public properties
        public string DyeCode
        {
            get { return this.textBoxCode.Text; }
            set { this.textBoxCode.Text = value; }
        }

        public string DyeName
        {
            get { return this.textBoxName.Text; }
            set { this.textBoxName.Text = value; }
        }

        public int DyeValve
        {
            get 
            {
                int valve = 0;
                try
                {
                    valve = Convert.ToInt32(this.textBoxValve.Text);
                }
                catch (System.Exception)
                {
                    valve = 0;
                }
                return valve; 
            }
            set { this.textBoxValve.Text = value.ToString(); }
        }
        #endregion

        #region public methods
        public void SetCodeReadOnly()
        {
            this.textBoxCode.ReadOnly = true;
        }
        #endregion

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (m_DyeType == DyeType.AUXILIARY)
            {
                if (String.IsNullOrEmpty(this.textBoxValve.Text))
                {
                    MessageBox.Show(Resources.ValveInputNull);
                    return;
                }

                int valve = -1;
                try
                {
                    valve = Convert.ToInt32(this.textBoxValve.Text);
                }
                catch (System.Exception)
                {
                    MessageBox.Show(Resources.ValveInputError);
                    return;
                }
            }

            if (String.IsNullOrEmpty(this.textBoxName.Text))
            {
                MessageBox.Show(Resources.NameInputError);
                return;
            }

            if (String.IsNullOrEmpty(this.textBoxCode.Text))
            {
                MessageBox.Show(Resources.CodeInputError);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
