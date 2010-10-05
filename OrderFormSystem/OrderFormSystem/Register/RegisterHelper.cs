using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace OrderFormSystem.Register
{
    class RegisterHelper
    {
        public const string SOFTWARE_NAME = "OrderFormSytem";   //注册表软件名
        public const string REGISTER_KEY = "OFSKey";            //注册表注册码名称
        public const string TRY_DAY = "OFSTryDate";             //注册表开始试用日期
        public const int TRY_TIME = 30;                         //试用期时间天数
        public const int TIP_TIME = 3;                         //提前提示天数
        
        ///获取磁盘序列号 
        /// <summary>
        /// 获取磁盘序列号
        /// </summary>
        static private string GetHardDiskID()
        {
            string hardDiskId = string.Empty;
            try
            {
                ManagementObjectSearcher cmicWmi = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                UInt32 tmpUint32 = 0;
                foreach (ManagementObject cmicWmiObj in cmicWmi.Get())
                {
                    tmpUint32 = Convert.ToUInt32(cmicWmiObj["signature"].ToString());
                }
                hardDiskId = tmpUint32.ToString();
            }
            catch { }
            return hardDiskId;
        }

        ///获取cpu序列号
        /// <summary>
        /// 获取cpu序列号
        /// </summary>
        static private string GetCPUID()
        {
            string cpuId = string.Empty;
            try
            {
                ManagementObjectSearcher Wmi = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject WmiObj in Wmi.Get())
                {
                    cpuId = WmiObj["ProcessorId"].ToString();
                }
            }
            catch { }
            return cpuId;
        }

        ///获取经过SHA1哈希之后的机器码
        /// <summary>
        /// 获取经过SHA1哈希之后的机器码
        /// </summary>
        static private string GetSHA1MachineCode(string code)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(code);
            SHA1 sha1 = SHA1.Create();
            return BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", "");
        }


        static public string GetMachineCode()
        {
            string cupID = GetCPUID();
            string hardDiskId = GetHardDiskID();
            return GetSHA1MachineCode(hardDiskId + cupID);
        }

        static public string GetRegisterCode()
        {
            return GetSHA1MachineCode(GetMachineCode());
        }

        #region 注册表
        //读取指定键路径的值
        static public string GetRegistData(string name)
        {
            string registData;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.OpenSubKey(SOFTWARE_NAME, true);
            registData = aimdir.GetValue(name).ToString();
            return registData;
        }

        //创建新值
        static public void WTRegedit(string name, string tovalue)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey software = hklm.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.CreateSubKey(SOFTWARE_NAME);
            aimdir.SetValue(name, tovalue);
        }

        //删除指定值
        static public void DeleteRegist(string name)
        {
            string[] aimnames;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.OpenSubKey(SOFTWARE_NAME, true);
            aimnames = aimdir.GetSubKeyNames();
            foreach (string aimKey in aimnames)
            {
                if (aimKey == name)
                    aimdir.DeleteSubKeyTree(name);
            }
        }

        //判断指定键是否存在
        static public bool IsRegPathExist()
        {
            bool _exit = false;
            string[] subkeyNames;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
            subkeyNames = software.GetSubKeyNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == SOFTWARE_NAME)
                {
                    _exit = true;
                    return _exit;
                }
            }
            return _exit;
        }

        static public bool IsRegeditExit(string name)
        {
            bool _exit = false;
            string[] subkeyNames;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.OpenSubKey(SOFTWARE_NAME, true);
            subkeyNames = aimdir.GetValueNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == name)
                {
                    _exit = true;
                    return _exit;
                }
            }
            return _exit;
        }
        #endregion
    }
}
