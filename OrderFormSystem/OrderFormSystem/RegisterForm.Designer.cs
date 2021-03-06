﻿namespace OrderFormSystem
{
    partial class RegisterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxRegisterCode = new System.Windows.Forms.TextBox();
            this.textBoxMachineCode = new System.Windows.Forms.TextBox();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.buttonTry = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.labelTips = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "机器码:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "注册码:(请凭机器码向软件供应商索取)";
            // 
            // textBoxRegisterCode
            // 
            this.textBoxRegisterCode.Location = new System.Drawing.Point(9, 129);
            this.textBoxRegisterCode.Name = "textBoxRegisterCode";
            this.textBoxRegisterCode.Size = new System.Drawing.Size(284, 21);
            this.textBoxRegisterCode.TabIndex = 3;
            // 
            // textBoxMachineCode
            // 
            this.textBoxMachineCode.Location = new System.Drawing.Point(9, 72);
            this.textBoxMachineCode.Name = "textBoxMachineCode";
            this.textBoxMachineCode.ReadOnly = true;
            this.textBoxMachineCode.Size = new System.Drawing.Size(284, 21);
            this.textBoxMachineCode.TabIndex = 1;
            // 
            // buttonRegister
            // 
            this.buttonRegister.Location = new System.Drawing.Point(9, 166);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(75, 23);
            this.buttonRegister.TabIndex = 4;
            this.buttonRegister.Text = "注册";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // buttonTry
            // 
            this.buttonTry.Location = new System.Drawing.Point(110, 166);
            this.buttonTry.Name = "buttonTry";
            this.buttonTry.Size = new System.Drawing.Size(75, 23);
            this.buttonTry.TabIndex = 5;
            this.buttonTry.Text = "试用";
            this.buttonTry.UseVisualStyleBackColor = true;
            this.buttonTry.Click += new System.EventHandler(this.buttonTry_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(218, 166);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(75, 23);
            this.buttonQuit.TabIndex = 6;
            this.buttonQuit.Text = "退出";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // labelTips
            // 
            this.labelTips.AutoSize = true;
            this.labelTips.Location = new System.Drawing.Point(7, 14);
            this.labelTips.Name = "labelTips";
            this.labelTips.Size = new System.Drawing.Size(0, 12);
            this.labelTips.TabIndex = 7;
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 197);
            this.Controls.Add(this.labelTips);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonTry);
            this.Controls.Add(this.buttonRegister);
            this.Controls.Add(this.textBoxMachineCode);
            this.Controls.Add(this.textBoxRegisterCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "RegisterForm";
            this.Text = "注册";
            this.Load += new System.EventHandler(this.RegisterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxRegisterCode;
        private System.Windows.Forms.TextBox textBoxMachineCode;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.Button buttonTry;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Label labelTips;
    }
}