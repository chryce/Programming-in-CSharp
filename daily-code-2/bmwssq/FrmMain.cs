using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;


namespace bmwssq
{
    public partial class frmMain : Form
    {
        Search sh = new Search();
        private bool _isSearch = false;//�Ƿ�����
        private string _condition = "��������ã�|������"; //Ĭ�ϵ��ı�
        private string _urlSrting;//��ҳurl
        private string _myCookie;//cookie
        private string _urlEncode;//ת����url

        /// <summary>
        /// ת����url
        /// </summary>
        public string UrlEncode
        {
            get { return _urlEncode; }
            set { _urlEncode = value; }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        #region ��ģ������

        /// <summary>
        /// load����
        /// </summary>
        private void setLoad()
        {
            MyRegistry reg = new MyRegistry();
            txtRegistryASCII.Text = reg.GetMachineCode();
            setSnik();
            setMain();
            setLink();
            setEmail();
            setTel();
            setMuSearch();
            gbEmail.Enabled = false;
            gbTel.Enabled = false;
            lswEmailResult.Visible = false;
            lswTelResult.Visible = false;
            ckbInNext.Enabled = false;
            ckbIsNotSoft.Visible = false;
            if (MyRegCode.IsReg)
            {
                lblsspRegText.Text = "ע���û���"+MyRegCode.RegName+"  ע�����䣺"+MyRegCode.Email+"  ע��ʱ�䣺"+MyRegCode.RegDate;
            }
        }

        /// <summary>
        /// ����Ƥ��
        /// </summary>
        private void setSnik()
        {
            switch (MyRegCode.SnikID)//�жϵ�ǰ��id������Ƥ��
            {
                case "0":
                    btnChangeSnik.PerformClick();
                    break;
                case "1":
                    mnuSnik1.PerformClick();
                    break;
                case "2":
                    mnuSnik2.PerformClick();
                    break;
                case "3":
                    mnuSnik3.PerformClick();
                    break;
                case "4":
                    mnuSnik4.PerformClick();
                    break;
                case "5":
                    mnuSnik5.PerformClick();
                    break;
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        private void setMain()
        {
            btnGoBack.Enabled = false;
            btnGoFront.Enabled = false;
            txtAddress.Text = MyRegCode.WebIndex;
            string url = txtAddress.Text;
            this.wbIndex.Navigate(url);
        }

        /// <summary>
        /// �������Ӳ���
        /// </summary>
        private void setLink()
        {
            txtSearchLinkHead.Text = "http://";
            txtSearchLinkPage1.Text = "1";
            txtSearchLinkPage2.Text = "100";
            txtSearchLinkEnd.Text = "";
            if (!ckbInNext.Checked)
            {
                txtWebPageNext.Enabled = false;
            }
        }

        /// <summary>
        /// ����Email����
        /// </summary>
        private void setEmail()
        {
            txtEmailKey.Text = _condition;
            txtEmailKey.Enabled = false;
            txtEmailNotIn.Text = _condition;
            txtEmailNotIn.Enabled = false;
            txtEmailSign1.Text = "@";
            txtEmailSign2.Text = ".";
            rbtnEmailKey.Checked = false;
            rbtnFiltrate.Checked = false;
        }

        /// <summary>
        /// ���õ绰����
        /// </summary>
        private void setTel()
        {
            ckbSearchHomeTel.Checked = false;
            ckbSearchMtel.Checked = true;
            ckbSearchInBaidu.Checked = false;
            if (ckbEmailOk.Checked)
            {
                ckbSearchInBaidu.Enabled = false;
            }
            txtSearchBaiduPage.Enabled = false;
            txtTelQuHao.Text = _condition;
            txtSearchBaiduPage.Text = "1";
            txtTelQuHao.Enabled = false;
            txtTelHaoDuan.Text = _condition;
            txtTelHaoDuan.Enabled = true;
            txtTelBaidu.Text = "";
            txtTelBaidu.Enabled = false;

        }

        /// <summary>
        /// ���ö������
        /// </summary>
        private void setMuSearch()
        {
            ckb3ceng.Checked = false;
            ckb4ceng.Checked = false;
            txt3ceng.Text = "";
            txt4ceng.Text = "";
            ckbMultilayerSearch.Checked = false;
            ckbMultilayerSearch.Visible = false;
            palVisible.Visible = false;
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private void beginSearch()
        {
            _isSearch = true;
            ckbEmailOk.Enabled = false;
            ckbSearchTelOk.Enabled = false;
            ckbInNext.Enabled = false;
            btnSearchBegin.Enabled = false;
        }
   
        /// <summary>
        /// ��������
        /// </summary>
        private void endSearch()
        {
            _isSearch = false;
            ckbEmailOk.Enabled = true;
            ckbSearchTelOk.Enabled = true;
            if (!ckbSearchInBaidu.Checked&&ckbSearchTelOk.Checked||ckbEmailOk.Checked)
            {
                ckbInNext.Enabled = true;
            }
            btnSearchBegin.Enabled = true;
            resultOut();
        }

        #endregion

        #region ������¼����룺������ҳ���

        private void frmMain_Load(object sender, EventArgs e) //�������
        {
            this.setLoad();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)//����رճ���
        {
            Application.Exit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)//�رմ���ʱ����xml
        {
            XMLOperate xo = new XMLOperate();
            xo.xmlSaveConfig();
        }

        private void txtAddress_KeyDown(object sender, KeyEventArgs e) //��ַ���س���
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOpen.PerformClick();
            }
        }

        private void txtSearchLinkHead_Enter(object sender, EventArgs e) //�ı����ý���
        {
            txtSearchLinkHead.SelectionStart = txtSearchLinkHead.Text.Length;
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e) //�˳���������
        {
            if (tabMain.SelectedIndex == 3)
            {

                DialogResult dr = MessageBox.Show("��ȷ���˳���?", "��ʾ��", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    Application.Exit();
                  }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e) //����վ
        {
            string url = this.txtAddress.Text;
            this.wbIndex.Navigate(url);
        }

        private void wbIndex_Navigating(object sender, WebBrowserNavigatingEventArgs e)//ҳ�����ǰ
        {
            this.btnGoBack.Enabled = false;
            this.btnGoFront.Enabled = false;
        }

        private void wbIndex_Navigated(object sender, WebBrowserNavigatedEventArgs e) //���µ�ַ��url
        {
            this.txtAddress.Text = this.wbIndex.Url.ToString();
        }

        private void btnSetIndex_Click(object sender, EventArgs e)//������ҳ
        {
            if (!MyRegCode.IsReg)
            {
                MessageBox.Show("��ע��汾���˹��ܲ����ã�\r\nע��������ޣ�",
                    "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            MyRegCode.WebIndex = txtAddress.Text;
            MessageBox.Show("���ѽ�\r\n"+MyRegCode.WebIndex+"\r\n��Ϊ��ҳ���´�ʹ�ý���Ч��",
                "���óɹ�",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        }

        private void btnGoBack_Click(object sender, EventArgs e)//����
        {
            if (!MyRegCode.IsReg)
            {
                MessageBox.Show("��ע��汾���˹��ܲ����ã�\r\nע��������ޣ�",
                    "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            wbIndex.GoBack();
        }

        private void btnGoFront_Click(object sender, EventArgs e)//ǰ��
        {
            if (!MyRegCode.IsReg)
            {
                MessageBox.Show("��ע��汾���˹��ܲ����ã�\r\nע��������ޣ�",
                    "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            wbIndex.GoForward();

        }

        private void wbIndex_NewWindow(object sender, CancelEventArgs e)//���´���
        {
            string NewURL = ((WebBrowser)sender).StatusText;
            wbIndex.Navigate(NewURL);
            //e.Cancel = true;
        }
        private void wbIndex_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) //��ҳ���ؽ���������
        {
            btnGoBack.Enabled = wbIndex.CanGoBack;
            btnGoFront.Enabled = wbIndex.CanGoForward;
        }

        private void frmMain_Resize(object sender, EventArgs e) //���������Сʱ������
        {
            if (ckbEmailOk.Checked && !ckbSearchTelOk.Checked)
            {
                lswEmailResult.Width = palResultpanel.Width;
            }
            else if (!ckbEmailOk.Checked && ckbSearchTelOk.Checked)
            {
                lswTelResult.Width = palResultpanel.Width;
            }
            else if (ckbSearchTelOk.Checked && ckbEmailOk.Checked)
            {
                lswEmailResult.Width = palResultpanel.Width / 2;
                lswTelResult.Width = palResultpanel.Width / 2;
            }
        }

        private void btnZhuce_Click(object sender, EventArgs e)//���ע��
        {
            if (txtZhuce.Text == "" || txtZhuce.Text.Length != 24)
            {
                MessageBox.Show("����д��ȷ��ע���룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("ע����ϣ������������Ч��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }


        }
 
        #endregion

        #region ������¼����룺�������

        private void ckbInNext_CheckedChanged(object sender, EventArgs e) //�Ƿ�ײ�����
        {
            if (ckbInNext.Checked)
            {
                txtWebPageNext.Enabled = true;
                ckbMultilayerSearch.Visible = true;
                ckbIsNotSoft.Visible = true;
            }
            else
            {
                txtWebPageNext.Enabled = false;
                ckbIsNotSoft.Visible = false;
                setMuSearch();
            }
        }

        private void ckbMultilayerSearch_CheckedChanged(object sender, EventArgs e) //ʹ�ö������
        {
            
            if (ckbMultilayerSearch.Checked)
            {
                if (!MyRegCode.IsReg)
                {
                    MessageBox.Show("��ע��汾���˹��ܲ����ã�\r\nע��������ޣ�",
                        "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ckbMultilayerSearch.Checked = false;
                    return;
                }
                palVisible.Visible = true;
                ckbIsNotSoft.Visible = false;
            }
            else
            {
                palVisible.Visible = false;
                ckbIsNotSoft.Visible = true;
            }
        }

        private void txtEmailKey_MouseDown(object sender, MouseEventArgs e)//Email�ؼ�������
        {
            txtEmailKey.SelectAll();
        }

        private void txtEmailNotIn_MouseDown(object sender, MouseEventArgs e)//Emailɸѡ����
        {
            txtEmailNotIn.SelectAll();
        }

        private void txtSearchBaiduPage_MouseDown(object sender, MouseEventArgs e)//�ٶ�����ҳ����
        {
            txtSearchBaiduPage.Text = "";
        }

        private void txtTelBaidu_MouseDown(object sender, MouseEventArgs e)//�ٶ������ؼ�������
        {
            txtTelBaidu.SelectAll();
        }

        private void txtTelHaoDuan_MouseDown(object sender, MouseEventArgs e)//�ֻ��Ŷ�����
        {
            txtTelHaoDuan.SelectAll();
        }

        private void txtTelQuHao_MouseDown(object sender, MouseEventArgs e)//�̻���������
        {
            txtTelQuHao.SelectAll();
        }

        private void txtSearchLinkPage1_MouseDown(object sender, MouseEventArgs e)//�������ӿ�ʼҳ��
        {
            txtSearchLinkPage1.Text = "";
        }

        private void txtSearchLinkPage2_MouseDown(object sender, MouseEventArgs e)//�������ӽ���ҳ��
        {
            txtSearchLinkPage2.Text = "";
        }

        private void ckbSearchInBaidu_CheckedChanged(object sender, EventArgs e) //�Ƿ��ڰٶ�����
        {
            if (ckbSearchInBaidu.Checked)
            {
                ckbInNext.Checked = false;
                ckbInNext.Enabled = false;
                txtTelBaidu.Enabled = true;
                txtSearchBaiduPage.Enabled = true;
            }
            else
            {
                ckbInNext.Enabled = true;
                txtTelBaidu.Enabled = false;
                txtSearchBaiduPage.Enabled = false;
            }
        }

        private void ckbEmailOk_CheckedChanged(object sender, EventArgs e) //�Ƿ�����Email
        {

            if (ckbEmailOk.Checked)
            {
                gbEmail.Enabled = true;
                ckbSearchInBaidu.Checked = false;
                ckbSearchInBaidu.Enabled = false;
                if (ckbSearchTelOk.Checked)
                {
                    lswEmailResult.Visible = true;
                    lswEmailResult.Width = palResultpanel.Width / 2;
                    lswTelResult.Width = palResultpanel.Width / 2;

                }
                else
                {
                    ckbInNext.Enabled = true;
                    lswEmailResult.Visible = true;
                    lswEmailResult.Width = palResultpanel.Width;
                }
            }
            else
            {
                if (!ckbSearchTelOk.Checked)
                {
                    ckbInNext.Enabled = false;
                    ckbInNext.Checked = false;
                }
                else
                {
                    ckbSearchInBaidu.Enabled = true;
                }
                setEmail();
                gbEmail.Enabled = false;
                lswEmailResult.Visible = false;
                lswTelResult.Width = palResultpanel.Width;
            }
        }

        private void ckbSearchTelOk_CheckedChanged(object sender, EventArgs e) //�Ƿ������绰
        {

            if (ckbSearchTelOk.Checked)
            {
                gbTel.Enabled = true;
                ckbSearchMtel.Checked = true;
                if (ckbEmailOk.Checked)
                {
                    lswTelResult.Visible = true;
                    lswEmailResult.Width = palResultpanel.Width / 2;
                    lswTelResult.Width = palResultpanel.Width / 2;
                }
                else
                {
                    ckbSearchInBaidu.Enabled = true;
                    ckbInNext.Enabled = true;
                    lswTelResult.Visible = true;
                    lswTelResult.Width = palResultpanel.Width;
                }
            }
            else
            {
                if (!ckbEmailOk.Checked)
                {
                    ckbInNext.Enabled = false;
                    ckbInNext.Checked = false;
                }
                setTel();
                gbTel.Enabled = false;
                lswTelResult.Visible = false;
                lswEmailResult.Width = palResultpanel.Width;
            }
        }

        private void rbtnEmailKey_CheckedChanged(object sender, EventArgs e) //�Ƿ�ʹ��Email�ؼ���
        {
            if (rbtnEmailKey.Checked)
            {
                txtEmailKey.Enabled = true;
            }
            else
            {
                txtEmailKey.Text = _condition;
                txtEmailKey.Enabled = false;
            }
        }

        private void rbtnFiltrate_CheckedChanged(object sender, EventArgs e) //�Ƿ����Email�ؼ���
        {
            if (rbtnFiltrate.Checked)
            {
                txtEmailNotIn.Enabled = true;
            }
            else
            {
                txtEmailNotIn.Text = _condition;
                txtEmailNotIn.Enabled = false;
            }
        }

        private void ckbSearchMtel_CheckedChanged(object sender, EventArgs e) //�Ƿ������ֻ�
        {
            if (ckbSearchMtel.Checked)
            {
                txtTelHaoDuan.Enabled = true;
            }
            else
            {
                txtTelHaoDuan.Text = _condition;
                txtTelHaoDuan.Enabled = false;
            }
        }

        private void ckbSearchHomeTel_CheckedChanged(object sender, EventArgs e) //�Ƿ������̻�
        {
            if (ckbSearchHomeTel.Checked)
            {
                txtTelQuHao.Enabled = true;
            }
            else
            {
                txtTelQuHao.Text = _condition;
                txtTelQuHao.Enabled = false;
            }
        }
        
        private void btnEmailClean_Click(object sender, EventArgs e) //����Emailѡ��
        {
            setEmail();
        }

        private void btnTelClean_Click(object sender, EventArgs e) //����telѡ��
        {
            setTel();
        }

        private void btnSearchLinkOk_Click(object sender, EventArgs e) //���������url
        {
            ListViewItem item = new ListViewItem();
            item.SubItems[0].Text = txtSearchLinkHead.Text;
            item.SubItems.Add(txtSearchLinkPage1.Text);
            item.SubItems.Add(txtSearchLinkPage2.Text);
            item.SubItems.Add(txtSearchLinkEnd.Text);
            lswSearchLinkView.Items.Add(item);
            setLink();
        }

        private void btnDelectLink_Click(object sender, EventArgs e) //ɾ����ѡ������
        {
            if (lswSearchLinkView.Items.Count > 0)
            {
                for (int i = this.lswSearchLinkView.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem item = this.lswSearchLinkView.SelectedItems[i];
                    this.lswSearchLinkView.Items.Remove(item);
                }
            }
        }

        private void btnSetDcssOK_Click(object sender, EventArgs e) //��������������
        {
            palVisible.Visible = false;
            ckbMultilayerSearch.Enabled = true;
        }

        private void btnSearchStop_Click(object sender, EventArgs e) //ֹͣ��������
        {
            _isSearch = false;
        }

        private void btnSearchBegin_Click(object sender, EventArgs e) //��ʼ����
        {
            beginSearch();
            _myCookie = sh.GetCookie(wbIndex);
            setAndSearch();
            endSearch();
        }

        private void btnGuolv_Click(object sender, EventArgs e) //����ɸѡ
        {
            ArrayList al;

            if (lswEmailResult.Items.Count < 1 && lswTelResult.Items.Count < 1)
            {
                MessageBox.Show("���Ƚ��������ٽ���ɸѡ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (lswEmailResult.Items.Count > 0)
                {
                    
                    al = new ArrayList();
                    foreach (ListViewItem item in lswEmailResult.Items)
                    {
                        al.Add(item.SubItems[0].Text);
                    }
                    lswEmailResult.Clear();
                    lswEmailResult.Columns.Insert(0, "Email��ַ", 150, HorizontalAlignment.Center);

                    for (int i = 0; i < al.Count; i++)
                    {
                        bool flag = true;
                        if (lswEmailResult.Items.Count < 1)
                        {
                            ListViewItem item = new ListViewItem();
                            item.SubItems[0].Text = al[i].ToString();
                            lswEmailResult.Items.Add(item);
                        }
                        else
                        {
                            foreach (ListViewItem item in lswEmailResult.Items)
                            {
                                if (item.SubItems[0].Text.Equals(al[i].ToString()))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                ListViewItem item = new ListViewItem();
                                item.SubItems[0].Text = al[i].ToString();
                                lswEmailResult.Items.Add(item);
                            }
 
                        }
                    }
                }
                if (lswTelResult.Items.Count > 0)
                {
                    
                    al = new ArrayList();
                    foreach (ListViewItem item in lswTelResult.Items)
                    {
                        al.Add(item.SubItems[0].Text);
                    }
                    lswTelResult.Clear();
                    lswTelResult.Columns.Insert(0, "�绰����", 150, HorizontalAlignment.Center);

                    for (int i = 0; i < al.Count; i++)
                    {
                        bool flag = true;
                        if (lswTelResult.Items.Count < 1)
                        {
                            ListViewItem item = new ListViewItem();
                            item.SubItems[0].Text = al[i].ToString();
                            lswTelResult.Items.Add(item);
                        }
                        else
                        {
                            foreach (ListViewItem item in lswTelResult.Items)
                            {
                                if (item.SubItems[0].Text.Equals(al[i].ToString()))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                ListViewItem item = new ListViewItem();
                                item.SubItems[0].Text = al[i].ToString();
                                lswTelResult.Items.Add(item);
                            }

                        }
                    }
                }
                MessageBox.Show("ɸѡ��ϣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnResuleOut_Click(object sender, EventArgs e) //����Email
        {
            if (!MyRegCode.IsReg)
            {
                MessageBox.Show("��ע��汾���˹��ܲ����ã�\r\nע��������ޣ�",
                    "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            resultOut(lswEmailResult);
        }

        private void btnTelResultOut_Click(object sender, EventArgs e) //����tel�ı�
        {
            if (!MyRegCode.IsReg)
            {
                MessageBox.Show("��ע��汾���˹��ܲ����ã�\r\nע��������ޣ�",
                    "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            resultOut(lswTelResult);
        }

        private void btnSearchLinkIn_Click(object sender, EventArgs e) //������ҳurl�ļ�
        {
            if (!MyRegCode.IsReg)
            {
                MessageBox.Show("��ע��汾���˹��ܲ����ã�\r\nע��������ޣ�",
                    "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            btnSearchLinkClean.PerformClick();
            for (int i = 0; i < lswSearchLinkView.Items.Count; i++)
            {
                lswSearchLinkView.Items.RemoveAt(0);
            }
            openFileDialog1.Filter = "(�ı��ĵ�)*.txt|";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = this.openFileDialog1.FileName;
                if (path != "")
                {
                    sh.ReadResultFromTxt(lswSearchLinkView, path);
                }
            }
        }

        private void btnSearchLinkClean_Click(object sender, EventArgs e) //�������url��
        {
            while (lswSearchLinkView.Items.Count > 0)
            {
                lswSearchLinkView.Items.RemoveAt(0);
            }
        }

        private void btnResultClean_Click(object sender, EventArgs e) //��ս������ʾ
        {
            lswEmailResult.Clear();
            lswEmailResult.Columns.Insert(0, "Email��ַ", 150, HorizontalAlignment.Center);
            lswEmailResult.Columns.Insert(1, "������ַ", 450, HorizontalAlignment.Center);
            lswTelResult.Clear();
            lswTelResult.Columns.Insert(0, "�绰����", 150, HorizontalAlignment.Center);
            lswTelResult.Columns.Insert(1, "������ַ", 450, HorizontalAlignment.Center);
        }


        #endregion

        #region �Զ��庯��

        /// <summary>
        /// �������TXT
        /// </summary>
        private void resultOut()
        {
            if (lswEmailResult.Items.Count > 0)
            {
                sh.WriteResultToTxt(lswEmailResult, "Email");
            }
            if (lswTelResult.Items.Count > 0)
            {
                sh.WriteResultToTxt(lswTelResult, "Tel");
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void setAndSearch()
        {
            try
            {
                if (ckbSearchInBaidu.Checked)
                {
                    searchInBaidu();
                }              
                else
                {
                    if (_myCookie == null)
                    {
                        MessageBox.Show("�������⣬�����½���ã�",
                                           "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (lswSearchLinkView.Items.Count > 0)
                    {
                        if (ckbInNext.Checked)
                        {
                            if (ckb3ceng.Checked)
                            {
                                searchNext2();
                            }
                            else
                            { 
                                searchNext();
                            }
                        }
                        else
                        {
                            setUrlStringAndSearch();
                        }
                    }
                    else
                    {
                        MessageBox.Show("����������ҳ�����","��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        /// <summary>
        /// �ڰٶ�����
        /// </summary>
        private void searchInBaidu()
        {
            ckbInNext.Checked = false;
            int page = Convert.ToInt32(txtSearchBaiduPage.Text) * 10;
            for (int i = 0; i < page; i = i + 10)
            {
                if (!_isSearch)
                {
                    break;
                }
                _urlSrting = @"http://www.baidu.com/s?lm=0&si=&rn=10&ie=gb2312&ct=0&wd="
                    + txtTelBaidu.Text + @"&pn=" + i.ToString() + @"&ver=0&cl=3";
                UrlEncode = sh.GetUrlEncode(_urlSrting);

                string regexTel = getRegexTel();
                string urlText = sh.GetUrlText(UrlEncode);
                if (!MyRegCode.IsReg && lswTelResult.Items.Count >= 50)
                {

                    MessageBox.Show("��ע��汾��ֻ������50����\r\nע��������ޣ�",
                            "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
                else
                {
                    ArrayList srs = sh.FindString(urlText, regexTel);
                    sh.WriteResult(srs, lswTelResult, UrlEncode);
                    sh.WriteResultToTxt(lswTelResult, "Tel");
                }
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        private void searchNext()
        {
            ArrayList al = null;

            #region �ؼ�ҳΪ��
            if (txtWebPageNext.Text == "")
            {
                _isSearch = false;
                MessageBox.Show("������ؼ�ҳ������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            #endregion
            else
            {
                string urlHead = "";
                string urlGuanjian = "";
                urlHead = sh.SubUrlHead(txtWebPageNext.Text);
                urlGuanjian = sh.SubUrlRegexStr(txtWebPageNext.Text);


                for (int i = 0; i < lswSearchLinkView.Items.Count; i++)
                {
                    if (!_isSearch)
                    {
                        break;
                    }
                    string urlStrHead = lswSearchLinkView.Items[i].SubItems[0].Text;
                    #region ��ʼҳ��Ϊ��
                    if (lswSearchLinkView.Items[i].SubItems[1].Text != "")//�����ʼҳ��Ϊ��
                    {
                        int sign1 = Convert.ToInt32(lswSearchLinkView.Items[i].SubItems[1].Text);
                        if (lswSearchLinkView.Items[i].SubItems[2].Text != "")
                        {
                            int sign2 = Convert.ToInt32(lswSearchLinkView.Items[i].SubItems[2].Text);
                            string urlStrEnd = lswSearchLinkView.Items[i].SubItems[3].Text;
                            for (int j = sign1; j <= sign2; j++)
                            {
                                if (!_isSearch)
                                {
                                    break;
                                }
                                _urlSrting = urlStrHead + j.ToString() + urlStrEnd;//��������url

                                UrlEncode = sh.GetUrlEncode(_urlSrting);

                                string urlText = sh.GetUrlText(UrlEncode, _myCookie);//�õ���ҳԴ��
                                al = sh.IsInNextSearch(urlText, urlGuanjian);//�õ��¼�url

                                for (int k = 0; k < al.Count; k++)//ѭ������
                                {
                                    try
                                    {
                                        if (!_isSearch)
                                        {
                                            break;
                                        }
                                        if (this.ckbIsNotSoft.Checked)
                                        {
                                            _urlSrting = al[k].ToString();
                                        }
                                        else
                                        {
                                            _urlSrting = urlHead + al[k].ToString();
                                        }
                                        UrlEncode = sh.GetUrlEncode(_urlSrting);
                                        string urlText1 = sh.GetUrlText(UrlEncode, _myCookie);
                                        searchMain(urlText1);
                                        Thread.Sleep(20);
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("�����ý���ҳ�룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    #endregion
                    #region ��ʼҳΪ��
                    else
                    {
                        UrlEncode = sh.GetUrlEncode(urlStrHead);
                        string urlText = sh.GetUrlText(UrlEncode, _myCookie);//�õ���ҳԴ��
                        al = sh.IsInNextSearch(urlText, urlGuanjian);//�õ��¼�url

                        for (int k = 0; k < al.Count; k++)//ѭ������
                        {
                            try
                            {
                                if (!_isSearch)
                                {
                                    break;
                                }
                                if (this.ckbIsNotSoft.Checked)
                                {
                                    _urlSrting = al[k].ToString();
                                }
                                else
                                {
                                    _urlSrting = urlHead + al[k].ToString();
                                }
                                UrlEncode = sh.GetUrlEncode(_urlSrting);
                                string urlText1 = sh.GetUrlText(UrlEncode, _myCookie);
                                searchMain(urlText1);
                                Thread.Sleep(20);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void searchNext2()
        {
            ArrayList al = null;//��鵽���¼�
            ArrayList al1 = null;//�¼����¼�

            if (txtWebPageNext.Text == "")
            {
                _isSearch = false;
                MessageBox.Show("������ؼ�ҳ������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string urlHead = "";//����ĵ�ַ
                string urlGuanjian = "";
                urlHead = sh.SubUrlHead(txtWebPageNext.Text);
                urlGuanjian = sh.SubUrlRegexStr(txtWebPageNext.Text);
                string urlHead1 = "";//����ĵ�ַ
                string urlGuanjian1 = "";
                urlHead1 = sh.SubUrlHead(this.txt3ceng.Text);
                urlGuanjian1 = sh.SubUrlRegexStr(txt3ceng.Text);


                for (int i = 0; i < lswSearchLinkView.Items.Count; i++)
                {
                    if (!_isSearch)
                    {
                        break;
                    }
                    string urlStrHead = lswSearchLinkView.Items[i].SubItems[0].Text;
                    if (lswSearchLinkView.Items[i].SubItems[1].Text != "")//�����ʼҳ��Ϊ��
                    {
                        int sign1 = Convert.ToInt32(lswSearchLinkView.Items[i].SubItems[1].Text);
                        if (lswSearchLinkView.Items[i].SubItems[2].Text != "")
                        {
                            int sign2 = Convert.ToInt32(lswSearchLinkView.Items[i].SubItems[2].Text);
                            string urlStrEnd = lswSearchLinkView.Items[i].SubItems[3].Text;
                            for (int j = sign1; j <= sign2; j++)
                            {
                                if (!_isSearch)
                                {
                                    break;
                                }
                                _urlSrting = urlStrHead + j.ToString() + urlStrEnd;//��������url

                                UrlEncode = sh.GetUrlEncode(_urlSrting);

                                string urlText = sh.GetUrlText(UrlEncode, _myCookie);//�õ���ҳԴ��
                                al = sh.IsInNextSearch(urlText, urlGuanjian);//�õ��¼�url

                                for (int k = 0; k < al.Count; k++)//ѭ������
                                {
                                    try
                                    {
                                        if (!_isSearch)
                                        {
                                            break;
                                        }
                                        _urlSrting = urlHead + al[k].ToString();
                                        UrlEncode = sh.GetUrlEncode(_urlSrting);
                                        string urlText1 = sh.GetUrlText(UrlEncode, _myCookie);

                                        al1 = sh.IsInNextSearch(urlText1, urlGuanjian1);//�õ��¼�url

                                        for (int l = 0; l < al.Count; l++)//ѭ������
                                        {
                                            try
                                            {
                                                if (!_isSearch)
                                                {
                                                    break;
                                                }
                                                _urlSrting = urlHead1 + al1[l].ToString();
                                                UrlEncode = sh.GetUrlEncode(_urlSrting);
                                                string urlText2 = sh.GetUrlText(UrlEncode, _myCookie);
                                                searchMain(urlText2);
                                                Thread.Sleep(20);
                                            }
                                            catch (Exception)
                                            {
                                                continue;
                                            }
                                        }
                                        Thread.Sleep(20);
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("�����ý���ҳ�룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        UrlEncode = sh.GetUrlEncode(urlStrHead);
                        string urlText = sh.GetUrlText(UrlEncode, _myCookie);//�õ���ҳԴ��
                        al = sh.IsInNextSearch(urlText, urlGuanjian);//�õ��¼�url

                        for (int k = 0; k < al.Count; k++)//ѭ������
                        {
                            try
                            {
                                if (!_isSearch)
                                {
                                    break;
                                }
                                _urlSrting = urlHead + al[k].ToString();
                                UrlEncode = sh.GetUrlEncode(_urlSrting);
                                string urlText1 = sh.GetUrlText(UrlEncode, _myCookie);
                                searchMain(urlText1);
                                Thread.Sleep(20);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// ������ҳurl������
        /// </summary>
        private void setUrlStringAndSearch()
        {
            for (int i = 0; i < lswSearchLinkView.Items.Count; i++)
            {
                if (!_isSearch)
                {
                    break;
                }

                string urlStrHead = lswSearchLinkView.Items[i].SubItems[0].Text;
                if (lswSearchLinkView.Items[i].SubItems[1].Text != "")
                {
                    int sign1 = Convert.ToInt32(lswSearchLinkView.Items[i].SubItems[1].Text);
                    if (lswSearchLinkView.Items[i].SubItems[2].Text != "")
                    {
                        int sign2 = Convert.ToInt32(lswSearchLinkView.Items[i].SubItems[2].Text);
                        string urlStrEnd = lswSearchLinkView.Items[i].SubItems[3].Text;

                        for (int j = sign1; j <= sign2; j++)
                        {
                            try
                            {
                                if (!_isSearch)
                                {
                                    break;
                                }
                                _urlSrting = urlStrHead + j.ToString() + urlStrEnd;
                                UrlEncode = sh.GetUrlEncode(_urlSrting);
                                string urlText = sh.GetUrlText(UrlEncode, _myCookie);

                                searchMain(urlText);
                            }
                            catch (Exception)
                            {
                                continue;
                            }    
                        }
                    }
                    else
                    {
                        MessageBox.Show("�����ý���ҳ�룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                else
                {
                    _urlSrting = urlStrHead;
                    UrlEncode = sh.GetUrlEncode(_urlSrting);
                    string urlText = sh.GetUrlText(UrlEncode, _myCookie);
                    searchMain(urlText);
                }
            }
        }

        /// <summary>
        /// ������������
        /// </summary>
        private void searchMain(string urlText)
        {
            if (!MyRegCode.IsReg && (lswTelResult.Items.Count >= 50||lswEmailResult.Items.Count>=50))
            {

                MessageBox.Show("��ע��汾��ֻ������50����\r\nע��������ޣ�",
                    "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _isSearch = false;
            }
            else
            {

                 
                
                //string urlText = sh.getUrlText(UrlEncode, _myCookie);
                string regexEmail = getRegexEmail();
                string regexTel = getRegexTel();
                if (ckbEmailOk.Checked && !ckbSearchTelOk.Checked)
                {
                    ArrayList srs = sh.FindString(urlText, regexEmail);
                    sh.WriteResult(srs, lswEmailResult, UrlEncode);
                    //sh.writeResultToTxt(lswEmailResult, "Email");
                }
                else if (!ckbEmailOk.Checked && ckbSearchTelOk.Checked)
                {
                    ArrayList srs = sh.FindString(urlText, regexTel);
                    sh.WriteResult(srs, lswTelResult, UrlEncode);
                    //sh.writeResultToTxt(lswTelResult, "Tel");
                }
                else if (ckbSearchTelOk.Checked && ckbEmailOk.Checked)
                {
                    ArrayList srs = sh.FindString(urlText, regexEmail);
                    sh.WriteResult(srs, lswEmailResult, UrlEncode);
                    //sh.writeResultToTxt(lswEmailResult, "Email");
                    ArrayList srs2 = sh.FindString(urlText, regexTel);
                    sh.WriteResult(srs2, lswTelResult, UrlEncode);
                    //sh.writeResultToTxt(lswTelResult, "Tel");
                }
                else if (!ckbSearchTelOk.Checked && !ckbEmailOk.Checked)
                {
                    MessageBox.Show("��ѡ��Ҫ����������!");
                }
            }
        }

        /// <summary>
        /// Email��������ʽ
        /// </summary>
        /// <returns>Email��������ʽ</returns>
        private string getRegexEmail()
        {
            string reg = @"([_a-z0-9-]+)";
            string sign1 = txtEmailSign1.Text;
            string sign2 = txtEmailSign2.Text;
            string regexEmail = @"([_a-z0-9-]+)" + sign1 + reg + sign2 + @"([^\s<>'"":;&#\*\\/]+[\w])";
            if (rbtnEmailKey.Checked)
            {
                regexEmail = @"([_a-z0-9-]+)" + sign1 + @"((" + txtEmailKey.Text + @"))" + sign2 + @"([^\s<>'"":;&#\*\\/]+[\w])";

            }
            if (rbtnFiltrate.Checked)
            {
                regexEmail = @"([_a-z0-9-]+)" + sign1 + @"([^(" + this.txtEmailNotIn.Text + @")])" + sign2 + @"([^\s<>'"":;&#\*\\/]+[\w])";
            }
            return regexEmail;
        }

        /// <summary>
        /// �绰��������ʽ
        /// </summary>
        /// <returns>�绰��������ʽ</returns>
        private string getRegexTel()
        {
            string regexTel = @"(1[3,5]\d{9})";
            if (!ckbSearchHomeTel.Checked && ckbSearchMtel.Checked)
            {
                if (!txtTelHaoDuan.Text.Equals(_condition))
                {
                    regexTel = @"((" + txtTelHaoDuan.Text + @")\d{8})";
                }
            }
            if (ckbSearchHomeTel.Checked && !ckbSearchMtel.Checked)
            {
                if (txtTelQuHao.Text.Equals(_condition))
                {
                    regexTel = @"(0\d{2,3}[\*,\-]?\d{7,8})";
                }
                else
                {
                    regexTel = @"((" + txtTelQuHao.Text + @"))[\*,\-]?\d{7,8})";
                }
            }

            if (ckbSearchHomeTel.Checked && ckbSearchMtel.Checked)
            {
                if (txtTelHaoDuan.Text.Equals(_condition) && txtTelQuHao.Text.Equals(_condition))
                {
                    regexTel = @"(1[3,5]\d{9})|(0\d{2,3}[\*,\-]?\d{7,8})";
                }
                else if (txtTelHaoDuan.Text.Equals(_condition) && !txtTelQuHao.Text.Equals(_condition))
                {
                    regexTel = @"((" + txtTelHaoDuan.Text + @")\d{8})|(0\d{2,3}[*,\-]?\d{7,8})";
                }
                else if (!txtTelHaoDuan.Text.Equals(_condition) && txtTelQuHao.Text.Equals(_condition))
                {
                    regexTel = @"(1[3,5]\d{9})|((" + txtTelQuHao.Text + @")[\*,\-]?\d{7,8})";
                }
                else
                {
                    regexTel = @"((" + txtTelHaoDuan.Text + @")\d{8})|((" + txtTelQuHao.Text + @")[\*,\-]?\d{7,8})";
                }
            }
            return regexTel;
        }

        /// <summary>
        /// �ֶ����������
        /// </summary>
        /// <param name="lv">�����</param>
        private void resultOut(ListView lv)
        {
            if (lv.Items.Count <= 0)
            {
                MessageBox.Show("���Ƚ��������ٵ����ļ���","��ʾ",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            string filepath = "";
            saveFileDialog1.Filter = "(�ı��ļ�)*.txt|";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = "d:\\";
            saveFileDialog1.OverwritePrompt = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                filepath = saveFileDialog1.FileName;
                StreamWriter sw = new StreamWriter(filepath);
                for (int i = 0; i < lv.Items.Count; i++)
                {
                    sw.WriteLine(lv.Items[i].SubItems[0].Text);//����Ҫд����ı�   
                }
                sw.Flush();
                sw.Close();
            }
        }

        #endregion

    }
}