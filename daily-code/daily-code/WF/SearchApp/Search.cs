using System;
using System.Windows.Forms;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading;

namespace SearchApp
{
    /// <summary>
    /// ��������ش���
    /// </summary>
    public class Search
    {

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="urlText">URLԴ�ļ�</param>
        /// <param name="zzbds">������ʽ</param>
        /// <returns>�����</returns>
        public ArrayList FindString(string urlText, string zzbds)
        {
            ArrayList searchResult = new ArrayList();
            Regex reg = new Regex(@"([\s<>'"":;����&#%\*\\/])(" + zzbds + @")([\s<>'"":;&#\*%\\/])", RegexOptions.IgnoreCase);
            Match match = reg.Match(urlText);
            while (match.Success)
            {
                string strResult = match.Result("$2").ToString();
                searchResult.Add(strResult);
                match = match.NextMatch();
                Thread.Sleep(10);
            }
            return searchResult;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns>����ַ����</returns>
        public ArrayList IsInNextSearch(string urlText,string keyWord)
        {
            ArrayList searchResult = new ArrayList();
            Regex reg = new Regex(@"(<a\s*href=.*)(" + keyWord + @"[^'""\s]*)('?""?.*>)", RegexOptions.IgnoreCase);
            Match match = reg.Match(urlText);
            while (match.Success)
            {
                string strResult = match.Result("$2").ToString();
                if (searchResult.Count > 0)
                {
                    bool flag = true; ;
                    for (int i = 0; i < searchResult.Count; i++)
                    {
                        if (strResult.Equals(searchResult[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        searchResult.Add(strResult);
                    }
                }
                else
                {
                    searchResult.Add(strResult);
                }
                match = match.NextMatch();
                Thread.Sleep(10);
            }
            return searchResult;

        }

        /// <summary>
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="url">��ҳURL</param>
        /// <returns>��ȡ�ĸ�Ŀ¼</returns>
        public string SubUrlHead(string url)
        {
            int last = url.LastIndexOf(@"/");
            string urlStrReturn = url.Substring(0, last + 1);
            return urlStrReturn;
        }

        /// <summary>
        /// ��ȡ�����ؼ���
        /// </summary>
        /// <param name="url">��ҳURL</param>
        /// <returns>���������Ĺؼ���</returns>
        public string SubUrlRegexStr(string url)
        {
            int last = url.LastIndexOf(@"/");
            string urlEnd = url.Substring(last, url.Length-1-last);
            int mid = urlEnd.IndexOf(@".");
            string urlStrReturn = urlEnd.Substring(1, mid);
            return urlStrReturn;
        }

        /// <summary>
        /// ��url���б���ת��
        /// </summary>
        /// <param name="urlstring">url</param>
        /// <returns>ת�����url</returns>
        public string GetUrlEncode(string urlstring)
        {
            int chfrom = Convert.ToInt32("4e00", 16);    //��Χ��0x4e00��0x9fff��ת����int��chfrom��chend��
            int chend = Convert.ToInt32("9fff", 16);

            string urlEncode = "";
            for (int i = 0; i < urlstring.Length; i++)
            {
                if (urlstring[i] >= chfrom && urlstring[i] <= chend)
                {
                    urlEncode += HttpUtility.UrlEncode(urlstring[i].ToString(), Encoding.Default);
                }
                else
                {
                    urlEncode += urlstring[i];
                }
            }
            return urlEncode;
        }

        /// <summary>
        /// д������
        /// </summary>
        /// <param name="al">�����</param>
        /// <param name="lv">Ҫд���ListView</param>
        public void WriteResult(ArrayList al, ListView lv, string url)
        {
            MyRegCode rc = new MyRegCode();
            if (al.Count > 0)
            {
                for (int i = 0; i < al.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    string temp = al[i].ToString();
                    item.SubItems[0].Text = temp;
                    item.SubItems.Add(url);
                    lv.Items.Add(item);
                    Thread.Sleep(10);
                }
            }
        }


        /// <summary>
        /// �Զ��������д��TXT�ļ�
        /// </summary>
        public void WriteResultToTxt(ListView lv, string findType)
        {
            string path = @"\" + findType + DateTime.Now.Date.ToShortDateString() + ".txt";

            string filePath = System.Environment.CurrentDirectory + path;
            FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            for (int i = 0; i < lv.Items.Count; i++)
            {
                streamWriter.WriteLine(lv.Items[i].SubItems[0].Text);
            }
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();

        }

        /// <summary>
        /// ����TXT�ļ�url�б�
        /// </summary>
        /// <param name="lv">��ʾ��listview</param>
        /// <param name="path">�����ļ���·��</param>
        public void ReadResultFromTxt(ListView lv, string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);
            while (!streamReader.EndOfStream)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems[0].Text = streamReader.ReadLine();
                lv.Items.Add(item);
            }
            streamReader.Close();
            fileStream.Close();
        }


        /// <summary>
        /// �õ���ҳԴ����
        /// </summary>
        /// <returns>��ҳԴ�ļ�</returns>
        public string GetUrlText(string url, string cook)
        {
            string urlText = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //�Լ�������Cookie
            request.Headers.Add("Cookie", cook);
            string postData ="id=2005&action=search&name=";
            byte[] byte1 = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = byte1.Length;

            Stream postStream = request.GetRequestStream();
            postStream.Write(byte1, 0, byte1.Length);
            postStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);
            urlText = sr.ReadToEnd();
            resStream.Close();
            sr.Close();
            return urlText;
            
        }

        /// <summary>
        /// ����cookie�õ�urlԴ��
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>��ҳԴ�ļ�</returns>
        public string GetUrlText(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);
            string urlText = sr.ReadToEnd();
            resStream.Close();
            sr.Close();
            return urlText;
        }

        /// <summary>
        /// �õ�Cookie
        /// </summary>
        /// <param name="wb">�����</param>
        /// <returns>Cookie</returns>
        public string GetCookie(WebBrowser wb)
        { 
            string cookieStr = null;
           
            if (wb.Document != null && wb.Document.Cookie!=null)
            {  
                cookieStr = wb.Document.Cookie;
            }
            return cookieStr;
        }

    }
}
