using System;
using System.Management;

using Microsoft.Win32;

namespace SearchApp
{
    public class MyRegistry
    {
        /// <summary>
        /// ȡ���豸Ӳ�̵ľ���
        /// </summary>
        /// <returns>Ӳ�̵ľ���</returns>
        private string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetWorkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("WIn32_logicaldisk.deviceid=\"d:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        /// <summary>
        /// ȡ��CPU�����к�
        /// </summary>
        /// <returns>CPU�����к�</returns>
        private string GetCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuCollection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuCollection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        /// <summary>
        /// ���ػ�����
        /// </summary>
        /// <returns>������</returns>
        public string GetMachineCode()
        {
            string ascii = GetDiskVolumeSerialNumber() + GetCpu();

            var registryAscii = RegInPassword(ascii);
            return registryAscii;
        }

        /// <summary>
        /// ���ɼ��ܵĻ�����
        /// </summary>
        /// <param name="ascii">���ܵĻ�����</param>
        public string RegInPassword(string ascii)
        {
            string regInPasswordString = "";
            char[] charID = new char[24];
            for (int i = 0; i < 24; i++)
            {
                charID[i] = Convert.ToChar(ascii.Substring(i, 1));
                int charIdToInt = Convert.ToInt32(charID[i]) + 5;
                if (charIdToInt >= 48 && charIdToInt <= 57 ||
                    charIdToInt >= 65 && charIdToInt <= 90 ||
                    charIdToInt >= 97 && charIdToInt <= 122
                    )
                {
                    regInPasswordString += Convert.ToChar(charIdToInt).ToString();
                }
                else
                {
                    if (charIdToInt > 122)
                    {
                        regInPasswordString += Convert.ToChar(charIdToInt - 10).ToString();
                    }
                    else
                    {
                        regInPasswordString += Convert.ToChar(charIdToInt - 9).ToString();
                    }
                }
            }
            return regInPasswordString;
        }

        /// <summary>
        /// �������ô���
        /// </summary>
        /// <returns>���ô���</returns>
        public int SetRegedit()
        {
            int tLong = 0;

            try
            {
                var res = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BMW");
                if (res == null)
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\BMW");
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\BMW", "UseTimes", 0);
                }
                else
                {
                    tLong = (Int32)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\BMW", "UseTimes", 0);
                    if (tLong < 100000000)
                    {
                        int time = tLong + 1;
                        Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\BMW", "UseTimes", time);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tLong;
        }
    }
}