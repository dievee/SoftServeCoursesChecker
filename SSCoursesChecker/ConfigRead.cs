using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SSCoursesChecker
{
     class ConfigRead
    {
        public string Index()
        {
            string config = "";//
            try
            {
                Form1 f1 = new Form1();
                using (StreamReader sr = new StreamReader(f1.inputpath)) 
                {
                    config = sr.ReadToEnd();
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("I can't find input.txt in program folder. Bye.");

                Environment.Exit(0);

            }


            config = DeleteComments(config);            
            config = DeleteExcess(config);

            return config;
        }
        public string DeleteExcess(string config)
        {
            config = config.Replace(" ", string.Empty);
            config = config.ToLower();

            config = config.Replace("\r", "");
            config = config.Replace("\n", "");

            return config;
        }
        public string DeleteComments(string config)
        {
            int i = 0;
            while(i != config.Length)
            {
                if (config[i] == '/' && config[i+1] =='/')
                {
                    config = config.Remove(i);
                    break;
                } 
                i++;
            }

            return config;
        }

    }
}
