using System;
using System.Collections.Generic;
using System.Text;

namespace bmwssq
{
    /// <summary>
    /// ע��ʵ����
    /// </summary>
    public class MyRegCode
    {
        private static bool _isReg = false;//�Ƿ�ע��

        public static bool IsReg
        {
            get { return MyRegCode._isReg; }
            set { MyRegCode._isReg = value; }
        }
        private static int _usedDays;//��������

        public static int UsedDays
        {
            get { return MyRegCode._usedDays; }
            set { MyRegCode._usedDays = value; }
        }
        private static string _regName="";//ע����

        public static string RegName
        {
            get { return MyRegCode._regName; }
            set { MyRegCode._regName = value; }
        }
        private static string _email="";//ע������

        public static string Email
        {
            get { return MyRegCode._email; }
            set { MyRegCode._email = value; }
        }
        private static string _machineCode;//������

        public static string MachineCode
        {
            get { return MyRegCode._machineCode; }
            set { MyRegCode._machineCode = value; }
        }
        private static string _regCode;//ע����

        public static string RegCode
        {
            get { return MyRegCode._regCode; }
            set { MyRegCode._regCode = value; }
        }
        private static string _regDate;//ע��ʱ��

        public static string RegDate
        {
            get { return MyRegCode._regDate; }
            set { MyRegCode._regDate = value; }
        }

        private static string _webIndex ="http://bmw-ruanjian.goofar.com";//��ҳ

        public static string WebIndex
        {
            get { return MyRegCode._webIndex; }
            set { MyRegCode._webIndex = value; }
        }
        private static string _snikID = "5";//Ƥ��id

        public static string SnikID
        {
            get { return MyRegCode._snikID; }
            set { MyRegCode._snikID = value; }
        }
    }
}
