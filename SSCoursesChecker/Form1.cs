using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using CsQuery;
using System.Web;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SSCoursesChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
        }
        public string inputpath = @"input.txt";
        public string outputpath = @"output.txt";
        public string programname = "Soft Serve courses checker";

        //public string inputpath = @"C:\Users\nex74\Desktop\input.txt";
        //public string outputpath = @"C:\Users\nex74\Desktop\output.txt";

        private void Form1_Load(object sender, EventArgs e)
        {
            if (AutorunCheck()) checkBox1.Checked = true;

            StreamData sd = new StreamData();
            ConfigRead cr = new ConfigRead();
            ParseConfig pc = new ParseConfig();

            List<List<string>> ConfigList = new List<List<string>>();
            List<List<string>> ParsedList = new List<List<string>>();
            List<string> ResultList = new List<string>();

            string config = cr.Index();
            ConfigList = pc.Index(config);

            ParsedList.Add( sd.Parse("technology"));
            ParsedList.Add( sd.Parse("city"));
            ParsedList.Add( sd.Parse("status"));
            ParsedList.Add( sd.Parse("link"));
            ResultList = FindLookingItems(ConfigList, ParsedList);

            OutputData(ResultList);

        }
        private bool AutorunCheck()
        {
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey);
            var autostartProgramNames = key.GetValueNames();

            foreach(string s in autostartProgramNames)
            { 
                if (s == programname) return true;
            }

            return false;
        }
        private void OutputData(List<string> list)
        {
            string lastdata = "";

            if (File.Exists(outputpath))
            {
                using (StreamReader sr = new StreamReader(outputpath))
                    lastdata = sr.ReadToEnd();
            }  
            else
            {
                //File.Create(outputpath);
                using (FileStream fstream = new FileStream(outputpath, FileMode.Create))
                {
                   // fstream.Close();
                    fstream.Dispose();
                }
                lastdata = "";
            }
                

            if (list.Count == 0)
            {
                string s = "\r\n" + DateTime.Now.ToString() + " There is't avalible courses.\r\n";
                File.WriteAllText(outputpath, s + lastdata);

                MessageBox.Show("Sorry but Soft Serve has not courses for you(");
            }
            else
            {
                list = SlipeElements(1, list);
                list[0] = "\r\n" + DateTime.Now.ToString();
                list.Add(lastdata);
                File.WriteAllLines(outputpath, list);

                MessageBox.Show("Looking courses are avalible! Let's check an output file.");
            }
        }
        private List<string> SlipeElements(int count, List<string> list )
        {
            for (int i = 0; i < count; i++)
                list.Add(" ");

            for (int i = list.Count - count - 1; i >= 0; i--)
            {
                list[i + count] = list[i];
            }

            return list;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                string ExePath = Application.ExecutablePath;
                RegistryKey Key = Registry.CurrentUser.
                    OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);

                Key.SetValue("Soft Serve courses checker", ExePath);
                Key.Close();
            }
            else
            {
                RegistryKey key = Registry.CurrentUser.
                    OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                key.DeleteValue("Soft Serve courses checker", false);
                key.Close();
            }
        }
        private List<string> FindLookingItems(List<List<string>> config, List<List<string>> parsed)
        {
            List<string> ResultList = new List<string>();
            List<string> AllConfigList = new List<string>();

            List<string> TechParsedList = new List<string>();
            List<string> CityParsedList = new List<string>();
            List<string> StatusParsedList = new List<string>();
            List<string> LinkParsedList = new List<string>();

            TechParsedList = parsed[0];
            CityParsedList = parsed[1];
            StatusParsedList = parsed[2];
            LinkParsedList = parsed[3];

            foreach (List<string> el in config)
            {
                foreach (string s in el)
                {
                    AllConfigList.Add(s);
                }
            }

            int i = 0;
            int numOfEqualEl = 0;
            for (i = 0; i < TechParsedList.Count; i++)
            {
                foreach (string el in AllConfigList)
                {
                    if (el == TechParsedList[i]) numOfEqualEl++;
                    if (el == CityParsedList[i]) numOfEqualEl++;
                    if (el == StatusParsedList[i]) numOfEqualEl++;
                }
                if (numOfEqualEl == 3)
                {
                    ResultList.Add(String.Format("{0} {1} {2} {3}", TechParsedList[i],
                                                                  CityParsedList[i],
                                                                  StatusParsedList[i],
                                                                  LinkParsedList[i]));
                }

                numOfEqualEl = 0;
            }

            return ResultList;
        }
    }
}
