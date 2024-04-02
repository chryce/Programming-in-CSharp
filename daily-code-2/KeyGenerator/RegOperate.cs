using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace KeyGenerator
{
    public class RegOperate
    {

        byte[] pkey = { 88, 55, 103, 24, 98, 46, 67, 29, 94, 19, 57, 118, 104, 15, 121,
            67, 93, 86, 24, 55, 102, 74, 98, 26, 67, 29, 19, 20, 49, 69, 73, 92 };

        byte[] IV = { 22, 76, 82, 77, 84, 31, 74, 47, 55, 102, 24, 98, 26, 67, 29, 99 };


        /// <summary>
        /// ����ע����
        /// </summary>
        /// <param name="machineCode">������</param>
        /// <returns>ע����</returns>
        public string GetRegCode(string machineCode)
        {
            string regCode = "";
            char[] charId = new char[24];
            for (int i = 0; i < 24; i++)
            {
                charId[i] = Convert.ToChar(machineCode.Substring(i, 1));
                int charIdToInt = Convert.ToInt32(charId[i]) + 5;
                if (charIdToInt >= 48 && charIdToInt <= 57 ||
                    charIdToInt >= 65 && charIdToInt <= 90 ||
                    charIdToInt >= 97 && charIdToInt <= 122
                    )
                {
                    regCode += Convert.ToChar(charIdToInt).ToString();
                }
                else
                {
                    if (charIdToInt > 122)
                    {
                        regCode += Convert.ToChar(charIdToInt - 10).ToString();
                    }
                    else
                    {
                        regCode += Convert.ToChar(charIdToInt - 9).ToString();
                    }
                }
            }
            return regCode;
        }

        /// <summary>
        /// д��xml�ļ�
        /// </summary>
        /// <param name="rt">ע����Ϣ��</param>
        public void XmlWrite(RegText rt)
        {
            XmlWriterSettings xmlset = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "   "
            };

            XmlWriter writer = XmlWriter.Create("key.xml", xmlset);

            writer.WriteStartElement("registry");
            writer.WriteElementString("Name", rt.RegName);
            writer.WriteElementString("Email", rt.Email);
            writer.WriteElementString("MachineCode", rt.MachineCode);
            writer.WriteElementString("RegCode", rt.RegCode);
            writer.WriteElementString("RegDate", DateTime.Now.Date.ToShortDateString());
            writer.WriteElementString("ShuoMing", "��ã���л��ʹ�� ��");

            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// ����xml�ļ�
        /// </summary>
        public void XmlToPassword()
        {
            try
            {
             
                RijndaelManaged managed = new RijndaelManaged();
                FileStream fsOut = File.Open("key", FileMode.OpenOrCreate, FileAccess.Write);
                FileStream fsIn = File.Open("key.xml", FileMode.OpenOrCreate, FileAccess.Read);
                ICryptoTransform cryptoTransform = managed.CreateEncryptor(pkey, IV);
                CryptoStream csDecrypt = new CryptoStream(fsOut, cryptoTransform, CryptoStreamMode.Write);
                BinaryReader br = new BinaryReader(fsIn);
                csDecrypt.Write(br.ReadBytes((int)fsIn.Length), 0, (int)fsIn.Length);
                csDecrypt.FlushFinalBlock();
                csDecrypt.Close();
                fsIn.Close();
                fsOut.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ���ܼ����ļ�
        /// </summary>
        public string ReadPasswordXml()
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            FileStream fsOut = File.Open("key", FileMode.Open, FileAccess.Read);
            CryptoStream csDecrypt = new CryptoStream(fsOut, myRijndael.CreateDecryptor(pkey, IV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(csDecrypt);
            //StreamWriter sw = new StreamWriter("key.xml");
            //sw.Write(sr.ReadToEnd()); 
            string outPassword = sr.ReadToEnd();
            //sw.Flush();
            //sw.Close();
            sr.Close();
            fsOut.Close();
            return outPassword;
        }
    }
}
