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
                //config = "Technologies: .NET;Java;devops; Cities: Kyiv;Lviv; Status: Register;Planned;";
                Environment.Exit(0);

            }

            return config;
        }

    }
}
