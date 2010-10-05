using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using OrderFormSystem.Properties;
using OrderFormSystem.DB;
using OrderFormSystem.Output;
using ModbusTCP;
using System.Threading;

namespace OrderFormSystem
{
    public partial class OFSForm : Form
    {
        #region Private var
        enum C_Mode
        {
            SYN = 0,
            ASY = 1
        }
        private const int MAX_RECORD_COUNT = 9;                 //配方最大记录数
        private const int MAX_ORDER_ID_LEN = 15;                //单号字符串最大长度
        private const int MAX_DYECODE_LEN = 15;                 //染助剂代码最大长度
        private const string m_fielPath = @".\user.db3";        //数据库路径
        private const string m_userTableName = @"User";         //用户表名
        private const string m_dyeTableName = @"Dye";           //染助剂表名
        private User m_user = null;                             //用户实例
        private ModbusTCP.Master m_ModbusTCPMaster;             //Modbus TCP通信辅助类实例
        private byte[] m_Data;                                  //通信字节数据
        private C_Mode m_mode = C_Mode.SYN;                     //通信模式
        #endregion

        #region Public Construction
        /// <summary>
        /// 构造函数
        /// </summary>
        public OFSForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Function Methods
        #region 注册
        private void RegisterBox(RegisterForm.RFState rfs)
        {
            RegisterForm reg = new RegisterForm(rfs);
            DialogResult dr = reg.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                this.Close();
            }
        }
        #endregion

        #region 用户
        private void NormalUserView()
        {
            this.groupBoxUserManage.Visible = false;
            this.groupBoxCaulSetting.Visible = false;
        }

        private void ManagerUserView()
        {
            this.groupBoxUserManage.Visible = true;
            this.groupBoxCaulSetting.Visible = true;

            this.buttonAddManageUser.Visible = false;
            this.buttonDelManageUser.Visible = false;
            this.listViewManager.Visible = false;
        }

        private void SuperUserView()
        {
            this.groupBoxUserManage.Visible = true;
            this.buttonAddManageUser.Visible = true;
            this.buttonDelManageUser.Visible = true;
            this.listViewManager.Visible = true;
        }

        private void LoadUserList(UserType ut)
        {
            //List<string> userNames = UserDBWrapper.GetNamesByType(ut);
            List<User> users = UserDB.GetAllRecords(ut);
            if (users == null)
            {
                return;
            }

            switch (ut)
            {
                case UserType.SuperUser:
                    break;
                case UserType.ManageUser:
                    this.listViewManager.Items.Clear();
                    break;
                case UserType.NormalUser:
                    this.listViewNormalUsers.Items.Clear();
                    break;
                default:
                    break;
            }

            foreach (User u in users)
            {
                switch (ut)
                {
                    case UserType.SuperUser:
                        break;
                    case UserType.ManageUser:

                        this.listViewManager.Items.Add(u.Name);
                        break;
                    case UserType.NormalUser:
                        this.listViewNormalUsers.Items.Add(u.Name);
                        break;
                    default:
                        break;
                }
            }
        }

        private void deleteUser(UserType ut)
        {
            ListView lv = null;
            switch (ut)
            {
                case UserType.SuperUser:
                    break;
                case UserType.ManageUser:
                    lv = this.listViewManager;
                    break;
                case UserType.NormalUser:
                    lv = this.listViewNormalUsers;
                    break;
                default:
                    break;
            }

            int count = lv.SelectedItems.Count;
            bool deleted = true;
            for (int i = count - 1; i >= 0; i--)
            {
                string name = lv.SelectedItems[i].Text;

                //if (!UserDBWrapper.Delete(name))
                if (!UserDB.Delete(name))
                {
                    deleted = false;
                }
                else
                {
                    MessageBox.Show(Resources.UserDeleted + name);
                }
            }
            if (deleted)
            {
                LoadUserList(ut);
            }
        }

        private bool AddUser(UserType ut)
        {
            UserForm uf = new UserForm();
            if (uf.ShowDialog() == DialogResult.OK)
            {
                //if (UserDBWrapper.Create(new User(uf.UserName, uf.PassWord, ut)))
                if (UserDB.Create(new User(uf.UserName, uf.PassWord, ut)))
                {
                    MessageBox.Show(Resources.UserAdded);
                    return true;
                }
                else
                {
                    MessageBox.Show(Resources.UserUnAdd);
                }
            }
            return false;
        }
        #endregion
        #region 染助剂
        private ListView GetListView(DyeType dt)
        {
            ListView lv = null;
            switch (dt)
            {
                case DyeType.DYE:
                    lv = this.listViewDye;
                    break;
                case DyeType.AUXILIARY:
                    lv = this.listViewAuxiliary;
                    break;
                default:
                    break;
            }
            return lv;
        }

        private void AddDye(DyeType dt)
        {
            DyeForm df = new DyeForm(dt);
            if (df.ShowDialog() == DialogResult.OK)
            {
                Dye d = new Dye(df.DyeCode, dt, df.DyeName, df.DyeValve);
                if (DyeDB.Create(d))
                {
                    RefreshDyeView(dt);
                }
                else
                {
                    string err = dt == DyeType.DYE ? Resources.AddDyeError : Resources.AddAuxiliaryError;
                    MessageBox.Show(err);
                }
            }
        }

        private void DelDye(DyeType dt)
        {
            ListView lv = GetListView(dt);
            if (lv == null)
            {
                return;
            }
            if (lv.SelectedItems.Count == 0)
            {
                return;
            }

            int index = lv.SelectedIndices[0];
            string code = lv.Items[index].Text;
            if (DyeDB.Delete(code))
            {
                RefreshDyeView(dt);
            }
            else
            {
                string err = dt == DyeType.DYE ? Resources.DelDyeError : Resources.DelAuxiliaryError;
                MessageBox.Show(err);
            }
        }

        private void EditDye(DyeType dt)
        {
            ListView lv = GetListView(dt);
            if (lv == null)
            {
                return;
            }
            if (lv.SelectedItems.Count == 0)
            {
                return;
            }

            int index = lv.SelectedIndices[0];
            DyeForm df = new DyeForm(dt);
            df.DyeCode = lv.Items[index].Text;
            df.DyeName = lv.Items[index].SubItems[1].Text;
            df.SetCodeReadOnly();

            if (df.ShowDialog() == DialogResult.OK)
            {
                Dye d = new Dye(df.DyeCode, dt, df.DyeName, df.DyeValve);
                if (DyeDB.Update(d))
                {
                    RefreshDyeView(dt);
                }
                else
                {
                    MessageBox.Show(Resources.UpdateError);
                }
            }
        }

        private void ClearDye(DyeType dt)
        {
            ListView lv = GetListView(dt);
            if (lv == null)
            {
                return;
            }
            if (lv.SelectedItems.Count == 0)
            {
                return;
            }

            if (DyeDB.DeleteAll(dt))
            {
                RefreshDyeView(dt);
            }
            else
            {
                string err = dt == DyeType.DYE ? Resources.ClearDyeError : Resources.ClearAuxiliaryError;
                MessageBox.Show(err);
            }
        }

        private string GetDyeName(string dyeCode)
        {
            Dye dye = DyeDB.GetRecord(dyeCode);
            if (dye == null)
            {
                MessageBox.Show(Resources.DyeCodeInputError);
                return String.Empty;
            }
            return dye.DyeName;
        }

        private int GetDyeVavle(string dyeCode)
        {
            Dye dye = DyeDB.GetRecord(dyeCode);
            if (dye == null)
            {
                MessageBox.Show(Resources.DyeCodeInputError);
                return 0;
            }
            return dye.DyeValve;
        }

        private void RefreshDNAView()
        {
            ListView lv = this.listViewDNA;

            lv.Items.Clear();

            List<Dye> dyeList = DyeDB.GetAllRecords();
            if (dyeList == null)
            {
                return;
            }
            foreach (Dye dye in dyeList)
            {
                ListViewItem item = new ListViewItem(dye.DyeCode);
                item.SubItems.Add(dye.DyeName);
                lv.Items.Add(item);
            }
        }

        private void RefreshDyeView(DyeType dt)
        {
            ListView lv = null;
            switch (dt)
            {
                case DyeType.DYE:
                    lv = this.listViewDye;
                    break;
                case DyeType.AUXILIARY:
                    lv = this.listViewAuxiliary;
                    break;
                default:
                    break;
            }

            lv.Items.Clear();

            List<Dye> dyeList = DyeDB.GetAllRecords(dt);
            if (dyeList == null)
            {
                return;
            }
            foreach (Dye dye in dyeList)
            {
                ListViewItem item = new ListViewItem(dye.DyeCode);
                item.SubItems.Add(dye.DyeName);
                item.SubItems.Add(dye.DyeValve.ToString());
                lv.Items.Add(item);
            }
        }
        #endregion
        #region 订单
        private Order GetInputData()
        {
            string orderID = this.textBoxOrderID.Text;
            if (String.IsNullOrEmpty(orderID))
            {
                MessageBox.Show(Resources.OrderIDEmpty);
                return null;
            }
            float liquorRatio1;
            try
            {
                liquorRatio1 = (float)Convert.ToDouble(this.textBoxLiquorRatio1.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show(Resources.LiquorRatioError);
                return null;
            }
            int yarnCount;
            try
            {
                yarnCount = Convert.ToInt32(this.textBoxYarnCount.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show(Resources.YarnCountError);
                return null;
            }
            string machineCode = this.textBoxMachineCode.Text;
            if (String.IsNullOrEmpty(machineCode))
            {
                MessageBox.Show(Resources.MachineCode);
                return null;
            }
            string colorNumber = this.textBoxColorNumber.Text;
            if (String.IsNullOrEmpty(colorNumber))
            {
                MessageBox.Show(Resources.ColorNumberEmpty);
                return null;
            }
            int creelCount;
            try
            {
                creelCount = Convert.ToInt32(this.textBoxCreelCount.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show(Resources.CreelCountError);
                return null;
            }
            float yarnWeight;
            try
            {
                yarnWeight = (float)Convert.ToDouble(this.textBoxYarnWeight.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show(Resources.YarnWeightError);
                return null;
            }
            float liquorRatio2;
            try
            {
                liquorRatio2 = (float)Convert.ToDouble(this.textBoxLiquorRatio1.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show(Resources.LiquorRatioError);
                return null;
            }
            float waterWeight;
            try
            {
                waterWeight = (float)Convert.ToDouble(this.textBoxWaterWeight.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show(Resources.WaterWeightError);
                return null;
            }
            string prescriptionCode = this.textBoxPrescriptionCode.Text;
            if (String.IsNullOrEmpty(prescriptionCode))
            {
                MessageBox.Show(Resources.PrescriptionEmpty);
                return null;
            }
            string pat = this.textBoxPattern.Text;
            if (String.IsNullOrEmpty(pat))
            {
                MessageBox.Show(Resources.PatternEmpty);
                return null;
            }

            string storeTime = String.IsNullOrEmpty(this.textBoxStoreTime.Text)?System.DateTime.Now.ToString():this.textBoxStoreTime.Text;

            string[] dyeCodes = new string[MAX_RECORD_COUNT];
            float[] densities = new float[MAX_RECORD_COUNT];

            int rowCount = this.dataGridViewDyeDetail.Rows.Count - 1;
            if (rowCount > MAX_RECORD_COUNT)
            {
                MessageBox.Show(Resources.LongPrescriptionError);
                return null;
            }
            for (int i = 0; i < rowCount; i++)
            {
                dyeCodes[i] = (string)this.dataGridViewDyeDetail.Rows[i].Cells[0].Value;

                try
                {
                    densities[i] = (float)Convert.ToDouble(this.dataGridViewDyeDetail.Rows[i].Cells[1].Value);
                }
                catch (System.Exception)
                {
                    MessageBox.Show(Resources.DensityError);
                    return null;
                }
            }
            string[] StepTexts = 
            {
                this.textBoxStep1.Text,
                this.textBoxStep2.Text,
                this.textBoxStep3.Text,
                this.textBoxStep4.Text,
                this.textBoxStep5.Text,
                this.textBoxStep6.Text,
                this.textBoxStep7.Text,
                this.textBoxStep8.Text,
                this.textBoxStep9.Text,
                this.textBoxStep10.Text,
                this.textBoxStep11.Text,
                this.textBoxStep12.Text,
                this.textBoxStep13.Text,
                this.textBoxStep14.Text
             };
            int stepCount = 0;
            if (!GetSteps(out stepCount))
            {
                MessageBox.Show(Resources.StepCountError);
                return null;
            }

            Order od = new Order(orderID,
                liquorRatio1,
                yarnCount,
                machineCode,
                colorNumber,
                creelCount,
                yarnWeight,
                liquorRatio2,
                waterWeight,
                prescriptionCode,
                pat,
                storeTime,
                stepCount,
                StepTexts,
                rowCount,
                dyeCodes,
                densities
                );
            return od;
        }

        private bool SaveOrder()
        {
            Order od = GetInputData();
            if (od == null)
            {
                //MessageBox.Show(Resources.CreateOrderFailed);
                return false;
            }
            if (OrderDB.Create(od))
            {
                //MessageBox.Show(Resources.SaveOrderSuccess);
                RefreshOrderIDView(this.listViewOrder);
                RefreshOrderContent(od.m_orderID);
                return true;
            }
            else
            {
                //MessageBox.Show(Resources.SaveOrderFailed);
                return false;
            }
        }

        private void DisplayOrderIDView(ListView lv)
        {
            if (lv.SelectedItems.Count == 0)
            {
                return;
            }
            int index = lv.SelectedIndices[0];
            string orderID = lv.Items[index].Text;
            RefreshOrderContent(orderID);
            this.textBoxOrderID.Focus();
            this.textBoxOrderID.SelectAll();
        }

        private void RefreshOrderIDView(ListView lv)
        {
            lv.Items.Clear();
            List<Order> ods = OrderDB.GetAllRecords();
            foreach (Order od in ods)
            {
                ListViewItem item = new ListViewItem(od.m_orderID);
                lv.Items.Add(item);
            }
        }

        private void RefreshOrderContent(string orderID)
        {
            Order od = OrderDB.GetRecord(orderID);
            if (od != null)
            {
                this.textBoxOrderID.Text = od.m_orderID;
                this.textBoxLiquorRatio1.Text = od.m_liquorRatio1.ToString();
                this.textBoxYarnCount.Text = od.m_yarnCount.ToString();
                this.textBoxMachineCode.Text = od.m_machineCode;
                this.textBoxColorNumber.Text = od.m_colorNumber;
                this.textBoxCreelCount.Text = od.m_creelCount.ToString();
                this.textBoxYarnWeight.Text = od.m_yarnWeight.ToString();
                this.textBoxLiquorRatio2.Text = od.m_liquorRatio2.ToString();
                this.textBoxWaterWeight.Text = od.m_waterWight.ToString();

                this.textBoxPrescriptionCode.Text = od.m_prescriptionCode;
                this.textBoxPattern.Text = od.m_pattern;
                this.textBoxStoreTime.Text = od.m_storeTime;

                TextBox[] steps = {
                                      this.textBoxStep1,
                                      this.textBoxStep2,
                                      this.textBoxStep3,
                                      this.textBoxStep4,
                                      this.textBoxStep5,
                                      this.textBoxStep6,
                                      this.textBoxStep7,
                                      this.textBoxStep8,
                                      this.textBoxStep9,
                                      this.textBoxStep10,
                                      this.textBoxStep11,
                                      this.textBoxStep12,
                                      this.textBoxStep13,
                                      this.textBoxStep14
                                  };
                for (int i = 0; i < od.m_stepCount; i++)
                {
                    steps[i].Text = od.m_stepCodes[i];
                }

                this.dataGridViewDyeDetail.Rows.Clear();
                for (int i = 0; i < od.m_prescriptionCount; i++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    foreach (DataGridViewColumn c in this.dataGridViewDyeDetail.Columns)
                    {
                        dr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell); //给行添加单元格
                    }
                    dr.Cells[0].Value = od.m_dyeCodes[i];
                    if (od.m_density[i] == 0)
                    {
                        dr.Cells[1].Value = null;
                    }
                    else
                    {
                        dr.Cells[1].Value = od.m_density[i];
                        dr.Cells[2].Value = od.m_density[i] * od.m_yarnWeight * od.m_liquorRatio2;
                    }
                    dr.Cells[3].Value = GetDyeVavle(od.m_dyeCodes[i]);
                    dr.Cells[4].Value = GetDyeName(od.m_dyeCodes[i]);

                    this.dataGridViewDyeDetail.Rows.Add(dr);
                }

                EnableOrderEdit(false);
            }
        }

        private void EmptyOrderEdit()
        {
            TextBox[] steps = {
                                      this.textBoxStep1,
                                      this.textBoxStep2,
                                      this.textBoxStep3,
                                      this.textBoxStep4,
                                      this.textBoxStep5,
                                      this.textBoxStep6,
                                      this.textBoxStep7,
                                      this.textBoxStep8,
                                      this.textBoxStep9,
                                      this.textBoxStep10,
                                      this.textBoxStep11,
                                      this.textBoxStep12,
                                      this.textBoxStep13,
                                      this.textBoxStep14
                                  };

            this.textBoxOrderID.Text = String.Empty;
            this.textBoxLiquorRatio1.Text = String.Empty;
            this.textBoxYarnCount.Text = String.Empty;
            this.textBoxMachineCode.Text = String.Empty;
            this.textBoxColorNumber.Text = String.Empty;
            this.textBoxCreelCount.Text = String.Empty;
            this.textBoxYarnWeight.Text = String.Empty;
            this.textBoxLiquorRatio2.Text = String.Empty;
            this.textBoxWaterWeight.Text = String.Empty;

            this.textBoxPrescriptionCode.Text = String.Empty;
            this.textBoxPattern.Text = String.Empty;
            this.textBoxStoreTime.Text = String.Empty;

            for (int i = 0; i < 14; i++)
            {
                steps[i].Text = String.Empty;
            }

            this.dataGridViewDyeDetail.Rows.Clear();
        }

        private void EnablePrintButton(bool enabled)
        {
            this.buttonPrint.Enabled = enabled;
        }

        private void EnableContextMenuStrip(ListView lv)
        {
            if (lv.SelectedItems.Count == 0)
            {
                this.contextMenuStripOrderIDView.Enabled = false;
                this.contextMenuStripWaitingOrderID.Enabled = false;
            }
            else
            {
                this.contextMenuStripOrderIDView.Enabled = true;
                this.contextMenuStripWaitingOrderID.Enabled = true;
            }
        }

        private void EnableOrderEdit(bool enabled)
        {
            TextBox[] steps = {
                                      this.textBoxStep1,
                                      this.textBoxStep2,
                                      this.textBoxStep3,
                                      this.textBoxStep4,
                                      this.textBoxStep5,
                                      this.textBoxStep6,
                                      this.textBoxStep7,
                                      this.textBoxStep8,
                                      this.textBoxStep9,
                                      this.textBoxStep10,
                                      this.textBoxStep11,
                                      this.textBoxStep12,
                                      this.textBoxStep13,
                                      this.textBoxStep14
                                  };
            
            //this.textBoxOrderID.Enabled = enabled;
            this.textBoxLiquorRatio1.Enabled = enabled;
            this.textBoxYarnCount.Enabled = enabled;
            this.textBoxMachineCode.Enabled = enabled;
            this.textBoxColorNumber.Enabled = enabled;
            this.textBoxCreelCount.Enabled = enabled;
            this.textBoxYarnWeight.Enabled = enabled;
            this.textBoxLiquorRatio2.Enabled = enabled;
            this.textBoxWaterWeight.Enabled = enabled;

            this.textBoxPrescriptionCode.Enabled = enabled;
            this.textBoxPattern.Enabled = enabled;
            this.textBoxStoreTime.Enabled = enabled;

            for (int i = 0; i < 14; i++)
            {
                steps[i].Enabled = enabled;
            }

            this.dataGridViewDyeDetail.ClearSelection();
            this.dataGridViewDyeDetail.Columns[1].ReadOnly = !enabled;
            EnableDyeCodeComboBox(false);

            EnablePrintButton(!enabled);
        }
        #endregion

        #region 步骤
        private bool StepsAllEmpty(int index, TextBox[] steps)
        {
            for (int i = index; i < steps.Length; i++)
            {
                if (!String.IsNullOrEmpty(steps[i].Text))
                {
                    return false;
                }
            }
            return true;
        }


        private void SetSteps(string[] texts, int count)
        {
            TextBox[] steps = {   
                                  this.textBoxStep1,
                                  this.textBoxStep2,
                                  this.textBoxStep3,
                                  this.textBoxStep4,
                                  this.textBoxStep5,
                                  this.textBoxStep6,
                                  this.textBoxStep7,
                                  this.textBoxStep8,
                                  this.textBoxStep9,
                                  this.textBoxStep10,
                                  this.textBoxStep11,
                                  this.textBoxStep12,
                                  this.textBoxStep13,
                                  this.textBoxStep14,
                              };
            for (int i = 0; i < count; i++)
            {
                steps[i].Text = texts[i];
            }
        }

        private bool GetSteps(out int stepCount)
        {
            TextBox[] steps = {   
                                  this.textBoxStep1,
                                  this.textBoxStep2,
                                  this.textBoxStep3,
                                  this.textBoxStep4,
                                  this.textBoxStep5,
                                  this.textBoxStep6,
                                  this.textBoxStep7,
                                  this.textBoxStep8,
                                  this.textBoxStep9,
                                  this.textBoxStep10,
                                  this.textBoxStep11,
                                  this.textBoxStep12,
                                  this.textBoxStep13,
                                  this.textBoxStep14,
                              };
            for (int i = 0; i < 14; i++)
            {
                if (String.IsNullOrEmpty(steps[i].Text) && StepsAllEmpty(i, steps))
                {
                    stepCount = i;
                    return true;
                }
                else if (String.IsNullOrEmpty(steps[i].Text) && !StepsAllEmpty(i, steps))
                {
                    stepCount = -1;
                    return false;
                }
            }
            stepCount = 14;
            return true;
        }
        #endregion

        #region 配方
        private void BindDyeCode()
        {
            DataTable dtDyeCode = new DataTable();
            dtDyeCode.Columns.Add("Value");
            dtDyeCode.Columns.Add("Name");
            DataRow drDyeCode;
            List<Dye> dyes = DyeDB.GetAllRecords();
            if (dyes == null)
            {
                this.comboBoxDyeCode.ValueMember = "Value";
                this.comboBoxDyeCode.DisplayMember = "Name";
                this.comboBoxDyeCode.DataSource = dtDyeCode;
                this.comboBoxDyeCode.DropDownStyle = ComboBoxStyle.DropDownList;
                return;
            }
            
            //drDyeCode = dtDyeCode.NewRow();
            //dtDyeCode.Rows.Add(drDyeCode);
            int recordCount = dyes.Count;
            for (int i = 0; i < recordCount; i++)
            {
                drDyeCode = dtDyeCode.NewRow();
                drDyeCode[0] = i.ToString();
                drDyeCode[1] = dyes[i].DyeCode;
                dtDyeCode.Rows.Add(drDyeCode);
            }

            this.comboBoxDyeCode.ValueMember = "Value";
            this.comboBoxDyeCode.DisplayMember = "Name";
            this.comboBoxDyeCode.DataSource = dtDyeCode;
            this.comboBoxDyeCode.DropDownStyle = ComboBoxStyle.DropDownList;
            //this.comboBoxDyeCode.AutoCompleteSource = AutoCompleteSource.ListItems;
            //this.comboBoxDyeCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void EnableDyeCodeComboBox(bool enable)
        {
            this.comboBoxDyeCode.Visible = enable;
        }

        private void SendOrderID(string df)
        {
            string orderID = this.textBoxOrderID.Text;
            byte[] data = System.Text.Encoding.Default.GetBytes(orderID);
            int len = orderID.Length;
            if (len > MAX_ORDER_ID_LEN)
            {
                MessageBox.Show("单号太长了");
                return;
            }
            for (int i = 0; i < len; i++)
            {
                byte[] dat = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)data[i]));
                byte[] ret = { };
                IOWrite(i, dat, ret);
            }
            for (int i = len; i < MAX_ORDER_ID_LEN; i++)
            {
                byte[] dat = { 0, 0 };
                byte[] ret = { };
                IOWrite(i, dat, ret);
            }
        }

        private void SendDyeCode(int addr, string dyeCode)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(dyeCode);
            int len = dyeCode.Length;
            if (len > MAX_DYECODE_LEN)
            {
                MessageBox.Show("染剂号太长了");
                return;
            }
            for (int i = 0; i < len; i++)
            {
                byte[] dat = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)data[i]));
                byte[] ret = { };
                IOWrite(addr + i, dat, ret);
            }
            for (int i = len; i < MAX_DYECODE_LEN; i++)
            {
                byte[] dat = { 0, 0 };
                byte[] ret = { };
                IOWrite(addr + i, dat, ret);
            }
        }

        private void SendInt(int addr, int fv)
        {
            byte[] data = BitConverter.GetBytes(fv);
            byte[] data2 = { data[1], data[0] };
            byte[] data1 = { data[3], data[2] };
            byte[] ret1 = { };
            byte[] ret2 = { };
            IOWrite(addr + 0, data1, ret1);
            IOWrite(addr + 1, data2, ret2);
        }

        private int GetIntValueFromDataGrid(int r, int c)
        {
            DataGridView dgv = this.dataGridViewDyeDetail;
            string sdisinity = (string)dgv.Rows[r].Cells[c].Value.ToString();
            if (sdisinity == null)
            {
                MessageBox.Show(Resources.NumberInputError);
                return -1;
            }
            try
            {
                int disinity = Convert.ToInt32(sdisinity);
                return disinity;
            }
            catch (System.Exception)
            {
                MessageBox.Show(Resources.NumberInputError);
                return -1;
            }
        }

        private void RefreshPrescriptionDetail()
        {
            float ratio = 0.0f;
            float yarnWeight = 0.0f;
            float density = 0.0f;

            string ratioText = this.textBoxLiquorRatio2.Text;
            string yarnWeightText = this.textBoxYarnWeight.Text;
            if (!String.IsNullOrEmpty(ratioText) && !String.IsNullOrEmpty(yarnWeightText))
            {
                try
                {
                    ratio = (float)Convert.ToDouble(ratioText);
                }
                catch (System.Exception)
                {
                    MessageBox.Show(Resources.NumberInputError);
                    return;
                }

                try
                {
                    yarnWeight = (float)Convert.ToDouble(yarnWeightText);
                }
                catch (System.Exception)
                {
                    MessageBox.Show(Resources.NumberInputError);
                    return;
                }
            }

            DataGridView dgv = this.dataGridViewDyeDetail;
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                string sdensity = (string)dgv.Rows[i].Cells[1].Value.ToString();
                try
                {
                    density = (float)Convert.ToDouble(sdensity);
                }
                catch (System.Exception)
                {
                    MessageBox.Show(Resources.DensityInputError);
                    return;
                }

                //density = (float)dgv.Rows[i].Cells[1].Value;

                float weight = ratio * yarnWeight * density;
                dgv.Rows[i].Cells[2].Value = weight.ToString();
            }
        }
        #endregion
        #region 通信
        // ------------------------------------------------------------------------
        // Event for response data
        // ------------------------------------------------------------------------
        private void MBmaster_OnResponseData(int ID, byte function, byte[] values)
        {
            // Ignore watchdog response data
            if (ID == 0xFF) return;

            // ------------------------------------------------------------------------
            // Identify requested data
            switch (ID)
            {
                case 1:
                    //grpData.Text = "Read coils";
                    m_Data = values;
                    break;
                case 2:
                    //grpData.Text = "Read discrete inputs";
                    m_Data = values;
                    break;
                case 3:
                    //grpData.Text = "Read holding register";
                    m_Data = values;
                    break;
                case 4:
                    //grpData.Text = "Read input register";
                    m_Data = values;
                    break;
                case 5:
                    //grpData.Text = "Write single coil";
                    break;
                case 6:
                    //grpData.Text = "Write multiple coils";
                    break;
                case 7:
                    //grpData.Text = "Write single register";
                    break;
                case 8:
                    //grpData.Text = "Write multiple register";
                    //MessageBox.Show("sent");
                    break;
            }
        }

        // ------------------------------------------------------------------------
        // Modbus TCP slave exception
        // ------------------------------------------------------------------------
        private void MBmaster_OnException(int id, byte function, byte exception)
        {
            string exc = "Modbus says error: ";
            switch (exception)
            {
                case Master.excIllegalFunction: exc += "Illegal function!"; break;
                case Master.excIllegalDataAdr: exc += "Illegal data adress!"; break;
                case Master.excIllegalDataVal: exc += "Illegal data value!"; break;
                case Master.excSlaveDeviceFailure: exc += "Slave device failure!"; break;
                case Master.excAck: exc += "Acknoledge!"; break;
                case Master.excMemParityErr: exc += "Memory parity error!"; break;
                case Master.excGatePathUnavailable: exc += "Gateway path unavailbale!"; break;
                case Master.excExceptionTimeout: exc += "Slave timed out!"; break;
                case Master.excExceptionConnectionLost: exc += "Connection is lost!"; break;
                case Master.excExceptionNotConnected: exc += "Not connected!"; break;
            }

            MessageBox.Show(exc, "Modbus slave exception");
        }

        private bool Connect()
        {
            string ipText = this.textBoxIP.Text;
            string portText = this.textBoxPort.Text;
            int port = Convert.ToInt32(portText);
            try
            {
                // Create new modbus master and add event functions
                m_ModbusTCPMaster = new Master(ipText, port);
                m_ModbusTCPMaster.OnResponseData += new ModbusTCP.Master.ResponseData(MBmaster_OnResponseData);
                m_ModbusTCPMaster.OnException += new ModbusTCP.Master.ExceptionData(MBmaster_OnException);
                return true;
            }
            catch (SystemException error)
            {
                MessageBox.Show(error.Message);
                return false;
            }
        }

        void IOWrite(int addr, byte[] data, byte[] ret)
        {
            if (m_mode == C_Mode.ASY)
            {
                IOWriteAsy(addr, 2, data);
            }
            else if (m_mode == C_Mode.SYN)
            {
                IOWriteSyn(addr, 2, data, ret);
            }
        }

        void IOWriteAsy(int addr, int len, byte[] data)
        {
            m_ModbusTCPMaster.WriteMultipleRegister(8, addr, len, data);
        }

        void IOWriteSyn(int addr, int len, byte[] data, byte[] ret)
        {
            m_ModbusTCPMaster.WriteMultipleRegister(8, addr, len, data, ret);
            if (ret == null)
            {
                MessageBox.Show("WriteMultipleRegister Error");
            }
        }

        private bool SendPrescriptionByModbusTCP(Order od)
        {
            if (!Connect())
            {
                return false;
            }

            SendOrderID(od.m_orderID);

            int recordCount = od.m_prescriptionCount;
            byte rc = (Byte)recordCount;
            byte[] data = { 0, rc };
            byte[] ret = { };
            IOWrite(30, data, ret);

            for (int i = 0; i < recordCount;i++ )
            {
                string dyeCode = od.m_dyeCodes[i];
                SendDyeCode(40 + 30 * i, dyeCode);
                int iWeight = od.m_weight[i];
                SendInt(60 + 30 * i, iWeight);

                int valve = od.m_valve[i];
                byte[] vdata = { 0, (Byte)valve };
                byte[] rest = { };
                IOWrite(69 + 30 * i, vdata, rest);
            }

            return true;
        }

        private bool SendPrescriptionByModbusTCP()
        {
            if (!Connect())
            {
                return false;
            }

            SendOrderID(this.textBoxOrderID.Text);

            int recordCount = this.dataGridViewDyeDetail.Rows.Count - 1;
            byte rc = (Byte)recordCount;
            byte[] data = { 0, rc };
            byte[] ret = { };
            IOWrite(30, data, ret);

            DataGridView dgv = this.dataGridViewDyeDetail;
            int rows = dgv.RowCount;
            if (rows <= 1)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < rows - 1; i++)
                {
                    int cells = dgv.Rows[i].Cells.Count;

                    string dyeCode = (string)dgv.Rows[i].Cells[0].Value;
                    SendDyeCode(40 + 30 * i, dyeCode);
                    int iWeight = GetIntValueFromDataGrid(i, 2);
                    SendInt(60 + 30 * i, iWeight);

                    string sValve = (string)dgv.Rows[i].Cells[3].Value.ToString();
                    int valve = Convert.ToInt32(sValve);
                    //int valve = (int)dgv.Rows[i].Cells[3].Value;
                    byte[] vdata = { 0, (Byte)valve };
                    byte[] rest = { };
                    IOWrite(69 + 30 * i, vdata, rest);
                }
            }
            return true;
        }
        #endregion
        #endregion

        #region Private Interface Methods
        #region 订单标签页
        private void tabControl1_Click(object sender, EventArgs e)
        {
            RefreshDNAView();
        }

        #region 订单View
        private void contextMenuStripOrderIDView_Opening(object sender, CancelEventArgs e)
        {
            EnableContextMenuStrip(this.listViewOrder);
        }

        private void contextMenuStripWaitingOrderID_Opening(object sender, CancelEventArgs e)
        {
            EnableContextMenuStrip(this.listViewWaitingOrderID);
        }

        private void ToolStripMenuItemDeleteOrder_Click(object sender, EventArgs e)
        {
            if (this.listViewOrder.SelectedItems.Count == 0)
            {
                MessageBox.Show(Resources.OrderIDViewNoSelected);
                return;
            }
            int index = this.listViewOrder.SelectedIndices[0];
            string orderID = this.listViewOrder.Items[index].Text;
            if (!OrderDB.Delete(orderID))
            {
                MessageBox.Show(Resources.OrderIDViewDeleteFailed);
            }
            else
            {
                RefreshOrderIDView(this.listViewOrder);
            }
        }

        private void ToolStripMenuItemDisplay_Click(object sender, EventArgs e)
        {
            DisplayOrderIDView(this.listViewOrder);
        }

        private void ToolStripMenuItemDeleteAll_Click(object sender, EventArgs e)
        {
            if (!OrderDB.ClearAllRecords())
            {
                MessageBox.Show(Resources.OrderIDViewDeleteFailed);
            }
            else
            {
                RefreshOrderIDView(this.listViewOrder);
            }
        }

        private void ToolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            if (this.listViewOrder.SelectedItems.Count == 0)
            {
                MessageBox.Show(Resources.OrderIDViewNoSelected);
                return;
            }
            int index = this.listViewOrder.SelectedIndices[0];
            string orderID = this.listViewOrder.Items[index].Text;
            int addedCount = this.listViewWaitingOrderID.Items.Count;
            for (int i = 0; i < addedCount; i++ )
            {
                if (this.listViewWaitingOrderID.Items[i].Text == orderID)
                {
                    MessageBox.Show(Resources.OrderIDViewAdded);
                    return;
                }
            }
            this.listViewWaitingOrderID.Items.Add(orderID);
        }

        private void ToolStripMenuItemShow_Click(object sender, EventArgs e)
        {
            DisplayOrderIDView(this.listViewWaitingOrderID);
        }

        private void ToolStripMenuItemRemove_Click(object sender, EventArgs e)
        {
            ListView lv = this.listViewWaitingOrderID;
            if (lv.SelectedItems.Count == 0)
            {
                MessageBox.Show(Resources.OrderIDViewNoSelected);
                return;
            }
            int index = lv.SelectedIndices[0];
            ListViewItem item = lv.Items[index];
            lv.Items.Remove(item);
            lv.Refresh();
        }

        private void ToolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            this.listViewWaitingOrderID.Items.Clear();
        }

        private void listViewWaitingOrderID_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DisplayOrderIDView(this.listViewWaitingOrderID);
        }

        private void listViewOrder_DoubleClick(object sender, EventArgs e)
        {
            DisplayOrderIDView(this.listViewOrder);
        }

        private void textBoxOrderID_TextChanged(object sender, EventArgs e)
        {
            EnableOrderEdit(true);
        }

        private void textBoxYarnWeight_TextChanged(object sender, EventArgs e)
        {
            RefreshPrescriptionDetail();
        }

        private void textBoxLiquorRatio2_TextChanged(object sender, EventArgs e)
        {
            RefreshPrescriptionDetail();
        }

        #endregion

        #region 染助剂View
        #endregion



        #region 配方详细DataGridView
        #region ComboBox for DyeCode
        private void comboBoxDyeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dyeCode = ((ComboBox)sender).Text;
            if (String.IsNullOrEmpty(dyeCode))
            {
                return;
            }
            this.dataGridViewDyeDetail.CurrentCell.Value = dyeCode;
            this.dataGridViewDyeDetail.CurrentCell.Tag = ((ComboBox)sender).Items.IndexOf(dyeCode);
            int r = this.dataGridViewDyeDetail.CurrentCell.RowIndex;
            this.dataGridViewDyeDetail.Rows[r].Cells[3].Value = GetDyeVavle(dyeCode).ToString();
            this.dataGridViewDyeDetail.Rows[r].Cells[4].Value = GetDyeName(dyeCode);
        }

        private void comboBoxDyeCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (this.comboBoxDyeCode.SelectedIndex >= this.comboBoxDyeCode.Items.Count)
            {
                this.comboBoxDyeCode.SelectedIndex = 0;
            }
            if (this.comboBoxDyeCode.SelectedIndex < 0)
            {
                this.comboBoxDyeCode.SelectedIndex = this.comboBoxDyeCode.Items.Count -1;
            }
            if (e.KeyCode == Keys.J)
            {
                this.comboBoxDyeCode.DroppedDown = true;
                this.comboBoxDyeCode.SelectedIndex++;
            }
            else if (e.KeyCode == Keys.K)
            {
                this.comboBoxDyeCode.DroppedDown = true;
                this.comboBoxDyeCode.SelectedIndex--;
            }
            else
            {
                this.dataGridViewDyeDetail.CurrentCell.Value = this.comboBoxDyeCode.Text;
                EnableDyeCodeComboBox(false);
                this.comboBoxDyeCode.DroppedDown = false;
            }
        }

        private void dataGridViewDyeDetail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            ComboBox dyeCB = this.comboBoxDyeCode;
            if (this.dataGridViewDyeDetail.CurrentCell.ColumnIndex == 0)
            {
                if (e.KeyCode == Keys.J)
                {
                    dyeCB.DroppedDown = true;
                    if (dyeCB.SelectedIndex == dyeCB.Items.Count - 1)
                    {
                        dyeCB.SelectedIndex = 0;
                    }
                    else
                    {
                        dyeCB.SelectedIndex++;
                    }
                }
                else if (e.KeyCode == Keys.K)
                {
                    this.comboBoxDyeCode.DroppedDown = true;
                    if (dyeCB.SelectedIndex == 0)
                    {
                        dyeCB.SelectedIndex = dyeCB.Items.Count - 1;
                    }
                    else
                    {
                        dyeCB.SelectedIndex--;
                    }
                }
                else
                {
                    this.dataGridViewDyeDetail.CurrentCell.Value = this.comboBoxDyeCode.Text;
                    EnableDyeCodeComboBox(false);
                    this.comboBoxDyeCode.DroppedDown = false;
                }
            }
        }

        private void dataGridViewDyeDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && !this.dataGridViewDyeDetail.Columns[1].ReadOnly)
            {
                DataGridView dgv = this.dataGridViewDyeDetail;
                Rectangle rect = dgv.GetCellDisplayRectangle(dgv.CurrentCell.ColumnIndex, dgv.CurrentCell.RowIndex, false);
                this.comboBoxDyeCode.Left = rect.Left;
                this.comboBoxDyeCode.Top = rect.Top;
                this.comboBoxDyeCode.Width = rect.Width;
                this.comboBoxDyeCode.Height = rect.Height;
                EnableDyeCodeComboBox(true);

                if (dgv.CurrentCell.Value == null)
                {
                    string dyeCode = String.Empty;
                    List<Dye> dyes = DyeDB.GetAllRecords();
                    if (dyes == null)
                    {
                        MessageBox.Show("染料和助剂资料缺失");
                    }
                    dyeCode = dyes[0].DyeCode;

                    this.comboBoxDyeCode.Text = dyeCode;
                }
                else
                {
                    string dyeCodeValue = dgv.CurrentCell.Value.ToString();
                    this.comboBoxDyeCode.Text = dyeCodeValue;
                }
            }
        }

        private void dataGridViewDyeDetail_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            EnableDyeCodeComboBox(false);
        }

        private void dataGridViewDyeDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = this.dataGridViewDyeDetail;
            int r = e.RowIndex;
            int c = e.ColumnIndex;
            if (c == 1)
            {
                RefreshPrescriptionDetail();
            }
        }

        #endregion
        #endregion

        #region 按钮操作区
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (SaveOrder())
            {
                MessageBox.Show(Resources.SaveOrderSuccess);
            }
            else
            {
                MessageBox.Show(Resources.SaveOrderFailed);
            }
        }

        private void buttonSentImmidiately_Click(object sender, EventArgs e)
        {
            //if (SaveOrder())
            //{
                if (SendPrescriptionByModbusTCP())
                {
                    if (SendPrescriptionByModbusTCP())
                    {
                        MessageBox.Show(Resources.OrderSendSuccess);
                    }
                }
            //}
            //else
            //{
            //    MessageBox.Show(Resources.SaveOrderFailed);
            //}
        }

        private void buttonSent_Click(object sender, EventArgs e)
        {
            ListView lv = this.listViewWaitingOrderID;
            int count = lv.Items.Count;
            for (int i = lv.Items.Count - 1; i >= 0; i-- )
            {
                Order od = OrderDB.GetRecord(lv.Items[i].Text);
                if (od != null && SendPrescriptionByModbusTCP(od))
                {
                    Thread.Sleep(1000 * 1000);
                    lv.Items.Remove(lv.Items[i]);
                    lv.Refresh();
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            EmptyOrderEdit();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            Order od = GetInputData();
            Output.Output op = new Output.PdfOutput(od);
            //Output.Output op = new Output.TxtOutput(od);
            op.PrintOut();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #endregion

        #region 资料标签页
        private void buttonAddDye_Click(object sender, EventArgs e)
        {
            AddDye(DyeType.DYE);
        }

        private void buttonDelDye_Click(object sender, EventArgs e)
        {
            DelDye(DyeType.DYE);
        }

        private void buttonClearDye_Click(object sender, EventArgs e)
        {
            ClearDye(DyeType.DYE);
        }

        private void buttonEditDye_Click(object sender, EventArgs e)
        {
            EditDye(DyeType.DYE);
        }

        private void buttonAddAuxiliary_Click(object sender, EventArgs e)
        {
            AddDye(DyeType.AUXILIARY);
        }

        private void buttonDelAuxiliary_Click(object sender, EventArgs e)
        {
            DelDye(DyeType.AUXILIARY);
        }

        private void buttonEditAuxiliary_Click(object sender, EventArgs e)
        {
            EditDye(DyeType.AUXILIARY);
        }

        private void buttonClearAuxiliary_Click(object sender, EventArgs e)
        {
            ClearDye(DyeType.AUXILIARY);
        }
        #endregion

        #region 系统标签页
        #region 用户
        private void buttonSavePasswd_Click(object sender, EventArgs e)
        {
            string oldPwd = this.textBoxOldPwd.Text;
            string newPwd = this.textBoxNewPwd.Text;
            string newPwdAgain = this.textBoxNewPwdAgain.Text;

            if (oldPwd == m_user.PassWord)
            {
                if (newPwd == newPwdAgain)
                {
                    m_user.PassWord = newPwd;
                    //if (UserDBWrapper.Update(m_user))
                    if (UserDB.Update(m_user))
                    {
                        MessageBox.Show(Resources.PwdChanged);

                    }
                    else
                    {
                        MessageBox.Show(Resources.PwdChangeFailed);
                        m_user.PassWord = oldPwd;
                    }
                }
                else
                {
                    MessageBox.Show(Resources.PasswdDiff);
                }
            }
            else
            {
                MessageBox.Show(Resources.LoginPasswordError);
            }
        }

        private void buttonAddNormalUser_Click(object sender, EventArgs e)
        {
            AddUser(UserType.NormalUser);
            LoadUserList(UserType.NormalUser);
        }

        private void buttonAddManageUser_Click(object sender, EventArgs e)
        {
            AddUser(UserType.ManageUser);
            LoadUserList(UserType.ManageUser);
        }

        private void buttonDelNormalUser_Click(object sender, EventArgs e)
        {
            deleteUser(UserType.NormalUser);
        }

        private void buttonDelManageUser_Click(object sender, EventArgs e)
        {
            deleteUser(UserType.ManageUser);
        }
        #endregion
        #region 网络设置
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (Connect())
            {
                MessageBox.Show(Resources.ConnectSuccess);
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OFSForm_Load(object sender, EventArgs e)
        {
            //注册判断
            string regCode = Register.RegisterHelper.GetRegisterCode();
            if (Register.RegisterHelper.IsRegPathExist()
                && Register.RegisterHelper.IsRegeditExit(Register.RegisterHelper.REGISTER_KEY)
                && Register.RegisterHelper.GetRegistData(Register.RegisterHelper.REGISTER_KEY) == regCode)
            {
            }
            else
            {
                if (Register.RegisterHelper.IsRegPathExist()
                    && Register.RegisterHelper.IsRegeditExit(Register.RegisterHelper.TRY_DAY))
                {
                    string tryDate = Register.RegisterHelper.GetRegistData(Register.RegisterHelper.TRY_DAY);
                    DateTime dt = Convert.ToDateTime(tryDate);
                    DateTime deadLine = dt.AddDays(Register.RegisterHelper.TRY_TIME);
                    DateTime tipDate = dt.AddDays(Register.RegisterHelper.TRY_TIME - Register.RegisterHelper.TIP_TIME);

                    DateTime now = DateTime.Now;
                    if(now.CompareTo(deadLine) > 0)
                    {
                        RegisterBox(RegisterForm.RFState.Invalid);
                    }
                    else if (now.CompareTo(tipDate) > 0)
                    {
                        RegisterBox(RegisterForm.RFState.Tips);
                    }
                }
                else
                {
                    RegisterBox(RegisterForm.RFState.FirstTime);
                }
            }


            ////弹出登录对话框
            //LoginForm lf = new LoginForm();
            //while (!lf.Success)
            //{
            //    if (lf.ShowDialog() != DialogResult.OK)
            //    {
            //        this.Close();
            //        return;
            //    }
            //}
            //m_user = lf.User;

            //LoadUserList(UserType.ManageUser);//加载管理用户
            //LoadUserList(UserType.NormalUser);//加载普通用户

            ////不同用户不同视图
            //switch (m_user.UserType)
            //{
            //    case UserType.SuperUser:
            //        SuperUserView();
            //        break;
            //    case UserType.ManageUser:
            //        ManagerUserView();
            //        break;
            //    case UserType.NormalUser:
            //        NormalUserView();
            //        break;
            //    default:
            //        break;
            //}

            RefreshOrderIDView(this.listViewOrder); //加载订单号数据
            RefreshDNAView();                       //加载染助剂视图
            RefreshDyeView(DyeType.DYE);            //加载染料视图(资料标签里面的)
            RefreshDyeView(DyeType.AUXILIARY);      //加载助剂视图(资料标签里面的)


            BindDyeCode();
            EnableDyeCodeComboBox(false);
            this.dataGridViewDyeDetail.Controls.Add(this.comboBoxDyeCode);
            EnablePrintButton(false);
            //OrderDB.ClearAllRecords();
        }
        #endregion
    }
}
