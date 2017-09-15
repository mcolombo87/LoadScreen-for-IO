using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace OI_for_OpenOrange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.ShowDialog();
        }

        private void loading()
        {
            string[] args = Environment.GetCommandLineArgs();

            progressBar1.PerformStep();
            string argumentTest = "";
            Boolean notShowSpash = false;
            foreach (string arg in args)
            {
                argumentTest = arg;
            }
            //Process to communicate with OrangeInstaller in Python.
            Process myProcess = new Process();

            //myProcess.StartInfo.Verb ="runas";
            switch (argumentTest)
            {
                case "--console":
                    myProcess.StartInfo.FileName = ".\\OI.exe";
                    myProcess.StartInfo.Arguments = "--console";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.UseShellExecute = true;
                    notShowSpash = true;
                    break;
                case "-C":
                    myProcess.StartInfo.FileName = ".\\OI.exe";
                    myProcess.StartInfo.Arguments = "--console";
                    myProcess.StartInfo.CreateNoWindow = false;
                    myProcess.StartInfo.UseShellExecute = false;
                    notShowSpash = true;
                    break;
                case "--debug":
                    myProcess.StartInfo.FileName = ".\\OI.exe";
                    myProcess.StartInfo.CreateNoWindow = false;
                    notShowSpash = true;
                    break;
                case "--test":
                    break;
                default:
                    myProcess.StartInfo.FileName = ".\\OrangeInstaller.exe";
                    myProcess.StartInfo.CreateNoWindow = false;
                    myProcess.StartInfo.UseShellExecute = true;
                    break;
            }

            Boolean err = false;
            int timeOut = 0;
            int lines = 0;
            try
            {
                lines = File.ReadLines(".\\install.log").Count();
            }
            catch
            {
                lines = 0;
            }
            myProcess.Start();
            if (notShowSpash == true)
            {
                this.terminate();
            }
            while (err == false && timeOut < 90)
            {
                try
                {
                    //var file = new FileStream("install.log", FileMode.Open, FileAccess.Read);
                    if (lines == File.ReadLines(".\\install.log").Count())
                    {
                        progressBar1.PerformStep();
                        progressBar1.Refresh();
                        timeOut += 1;
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    err = true;
                }
            }
            this.Refresh();
            if (progressBar1.Value < 100)
            {
                while (progressBar1.Value < 100)
                {
                    progressBar1.PerformStep();
                    progressBar1.Refresh();
                }
            }

            this.terminate();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            loading();
        }

        private void terminate()
        {
            try
            {
                this.Dispose();
                this.Close();
            }
            catch
            {
                this.Close();
            }
        }
    }

}
