﻿using HuUtils.Service;

using LibGit2Sharp;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuUtils
{
    public partial class GitOp : Form
    {
        private const int WmSyscommand = 0x0112;          //点击窗口左上角那个图标时的系统信息
        private const int ScMove = 0xF010;                //移动信息
        private const int Htcaption = 0x0002;             //表示鼠标在窗口标题栏时的系统信息
        private const int WmNchittest = 0x84;             //鼠标在窗体客户区（除了标题栏和边框以外的部分）时发送的消息
        private const int Htclient = 0x1;                 //表示鼠标在窗口客户区的系统消息
        private const int ScMaximize = 0xF030;            //最大化信息
        private const int ScMinimize = 0xF020;            //最小化信息

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case WmSyscommand:
                    if (m.WParam == (IntPtr)ScMaximize)
                    {
                        m.WParam = (IntPtr)ScMinimize;
                    }
                    break;

                case WmNchittest: //如果鼠标移动或单击
                    base.WndProc(ref m);//调用基类的窗口过程——WndProc方法处理这个消息
                    if (m.Result == (IntPtr)Htclient)//如果返回的是HTCLIENT
                    {
                        m.Result = (IntPtr)Htcaption;//把它改为HTCAPTION
                        return;//直接返回退出方法
                    }
                    break;
            }
            base.WndProc(ref m);//如果不是鼠标移动或单击消息就调用基类的窗口过程进行处理
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)

        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;
            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        this.Close();//esc关闭窗体
                        break;
                }
            }
            return false;
        }

        public GitOp()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private delegate void CloneDele(string url, string destPath);

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialogGit.ShowDialog())
            {
                txtGitDir.Text = folderBrowserDialogGit.SelectedPath;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialogGit.ShowDialog())
            {
                txtGitPath.Text = folderBrowserDialogGit.SelectedPath;
            }
        }

        //Clone 基目录
        private void BtnPullSourceUrl_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialogGit.ShowDialog())
            {
                txtDestBasePath.Text = folderBrowserDialogGit.SelectedPath;
            }
        }

        //Git地址文件地址
        private void BtnSelectUrlsFile_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialogGit.ShowDialog())
            {
                txtUrlsPath.Text = openFileDialogGit.FileName;
            }
        }

        public static string GetDestPath(string path)
        {
            string[] strs = path.Split('/');
            int len = strs.Length;
            var newPath = strs[len - 1];
            if (newPath.EndsWith(".git"))
            {
                newPath = newPath.Replace(".git", "");
            }
            return newPath;
        }

        public void Fetch(string path)
        {
            try
            {
                string logMessage = "";
                using (var repo = new Repository(path))
                {
                    var remote = repo.Network.Remotes["origin"];
                    var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                    Commands.Fetch(repo, remote.Name, refSpecs, new FetchOptions
                    {
                        TagFetchMode = TagFetchMode.All,
                        OnTransferProgress = progress =>
                        {
                            string log = string.Format(CultureInfo.InvariantCulture, "{4}-{0}-{1}/{2}, {3} bytes", remote.Name, progress.ReceivedObjects, progress.TotalObjects, progress.ReceivedBytes, remote.PushUrl);
                            txtLog.AppendText(log + Environment.NewLine + Environment.NewLine);
                            return true;
                        }
                    }, logMessage);
                    txtLog.AppendText((remote.PushUrl ?? path) + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(path);
            }
        }

        public void TaskFetch(string directory, int taskCount)
        {
            Queue<string> list = new Queue<string>();

            string[] directories = Directory.GetDirectories(directory);
            foreach (string path in directories)
            {
                list.Enqueue(path);
            }

            Task[] tasks = new Task[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    while (list.Any())
                        Fetch(list.Dequeue());
                });
                tasks[i] = task;
            }
            Task.WaitAll(tasks);
        }

        private void Callback(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                txtLog.AppendText($"{result.AsyncState} Clone Success..." + Environment.NewLine);
            }
        }

        private void Clone(string url, string destPath)
        {
            try
            {
                CloneOptions options = new CloneOptions
                {
                    OnCheckoutProgress = (path, completedSteps, totalSteps) =>
                    {
                        txtLog.AppendText(url + " " + path + " " + completedSteps + " " + totalSteps + Environment.NewLine);
                    },
                    OnProgress = serverProgressOutput =>
                    {
                        txtLog.AppendText($"{url} Progress: " + serverProgressOutput + Environment.NewLine);
                        return true;
                    },
                };
                txtLog.AppendText("Clone Start:" + url + Environment.NewLine);
                string clonedRepoPath = Repository.Clone(url, destPath, options);
                using (Repository repo = new Repository(clonedRepoPath))
                {
                    repo.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void TaskClone(string urlLines, string destPath)
        {
            List<string> list = new List<string>();
            string[] lines = File.ReadAllLines(urlLines);
            foreach (string pathStr in lines)
            {
                list.Add(pathStr);
            }
            List<Task> tasks = new List<Task>();

            foreach (string gitUrl in list)
            {
                if (string.IsNullOrWhiteSpace(gitUrl) || string.IsNullOrWhiteSpace(destPath))
                {
                    continue;
                }

                string path = Path.Combine(destPath, GetDestPath(gitUrl).TrimEnd('.'));

                if (Directory.Exists(path))
                {
                    continue;
                }
                txtLog.AppendText($"{gitUrl}"   + Environment.NewLine);
                Task task = Task.Factory.StartNew(() =>
                {
                    CloneDele cloneDele = Clone;
                    cloneDele.BeginInvoke(gitUrl, path, Callback, gitUrl);
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        private void BtnGetUrl_Click(object sender, EventArgs e)
        {
            try
            {
                SortedBindingList<GitInfoPath> gitPaths = new SortedBindingList<GitInfoPath>();
                if (!String.IsNullOrWhiteSpace(txtGitPath.Text))
                {
                    var directories = Directory.GetDirectories(txtGitPath.Text);
                    foreach (string directory in directories)
                    {
                        var gitInfoPath = new GitInfoPath();
                        using (var repo = new Repository(directory))
                        {
                            var remote = repo.Network.Remotes["origin"];
                            gitInfoPath.DirName = directory.Substring(directory.LastIndexOf('\\') + 1);
                            gitInfoPath.GitPath = remote.PushUrl;
                            gitPaths.Add(gitInfoPath);
                        }
                    }
                }
                dgGitView.DataSource = gitPaths;
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGitDir.Text))
            {
                MessageBox.Show(@"请选择文件夹", @"Git 库 目录", MessageBoxButtons.AbortRetryIgnore);
                return;
            }
            string[] directories = Directory.GetDirectories(txtGitDir.Text);
            int taskCount = (directories.Length / 5) + 1;
            TaskFetch(txtGitDir.Text, taskCount);
        }

        /// <summary>
        /// 拉取代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPull_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrlsPath.Text) || string.IsNullOrWhiteSpace(txtDestBasePath.Text))
            {
                MessageBox.Show(@"请选择文件夹", @"Git 库 目录", MessageBoxButtons.AbortRetryIgnore);
                return;
            }

            Thread.Sleep(2000);
            TaskClone(txtUrlsPath.Text, txtDestBasePath.Text);
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDestBasePath.Text))
            {
                MessageBox.Show("Dest 不可为空");
            }
            string[] directories = Directory.GetDirectories(txtDestBasePath.Text);
            foreach (string dir in directories)
            {
                string[] childDirs = Directory.GetDirectories(dir);
                string[] files = Directory.GetFiles(dir);
                if (childDirs.Length == 1 && files.Length == 0)
                {
                    FileSystem.DeleteDirectory(childDirs[0], UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                    FileSystem.DeleteDirectory(dir, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                }
            }
            MessageBox.Show("Clean Ok!!");
        }
    }

    public class GitInfoPath
    {
        public string DirName { get; set; }

        public string GitPath { get; set; }
    }
}