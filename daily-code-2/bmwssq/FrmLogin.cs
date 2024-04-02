using System;
using System.Windows.Forms;

namespace bmwssq
{
    /// <summary>
    /// ��½�������
    /// </summary>
    public partial class FrmLogin : Form
    {
        private MyRegistry rsy = new MyRegistry();
        private XMLOperate xo = new XMLOperate();

        public FrmLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��½�ж���
        /// </summary>
        private void LoginLoad()
        {
            MyRegCode.UsedDays = rsy.SetRegedit();
            MyRegCode.MachineCode = rsy.GetMachineCode();
            if (xo.xmlReadKey() && MyRegCode.RegCode.Equals(rsy.regInPassword(MyRegCode.MachineCode)))
            {
                xo.xmlLoadConfig();
                MyRegCode.IsReg = true;
                this.lblLoginSay.Text = "ע���\r\n\r\n ע���û���\r\n  " + MyRegCode.RegName + "\r\n\r\n ע�����ڣ�\r\n  " + MyRegCode.RegDate;
            }
            else
            {
                this.lblLoginSay.Text = "δע���\r\n\r\n �����ã�10��\r\n\r\n �����ã�" + MyRegCode.UsedDays + "��";
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)//�������
        {
            LoginLoad();
            tmLogin.Enabled = true;
            tmLogin.Interval = 5000;
        }

        private void tmLogin_Tick(object sender, EventArgs e)//ʱ���¼�
        {
            this.Close();
        }

        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)//����ر�
        {
            if (MyRegCode.UsedDays >= 10 && !MyRegCode.IsReg)//�ж����ô������Ƿ�ע��
            {
                Application.Exit();
            }
            else
            {
                frmMain fm = new frmMain();
                fm.Show();
            }
        }

        private void FrmLogin_Click(object sender, EventArgs e)//���嵥���¼�
        {
            tmLogin.Enabled = false;
            this.Close();
        }
    }
}