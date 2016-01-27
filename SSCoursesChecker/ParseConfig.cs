using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCoursesChecker
{
    class ParseConfig
    {
        ConfigRead cr = new ConfigRead();
        List<List<string>> ConfigList = new List<List<string>>();

        List<string> TechMembers = new List<string>();
        List<string> CityMembers = new List<string>();
        List<string> StatusMembers = new List<string>();

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
            while (i != config.Length)
            {
                if (config[i] == '/' && config[i + 1] == '/' &&
                    config[i + 2] == '/' && config[i + 3] == '/')
                {
                    config = config.Remove(i);
                    break;
                }
                i++;
            }

            return config;
        }

        public List<List<string>> Index(string config)
        {
            int numberOfDoneCatt = 0;

            config = DeleteComments(config);
            config = DeleteExcess(config);

            for (int i = 0; i < config.Length; i++)
            {
                string part = "";
                int j = i;
                int length = 0;

                while (config[j] != ':' && numberOfDoneCatt < 3)
                {
                    part += config[j];
                    j++;
                }
                length = i + part.Length;
                switch (part)
                {
                    case "technologies":

                        string membersstr = Cut(config, length);
                        TechMembers = FindMembers(membersstr);
                        ConfigList.Add(TechMembers);
                        numberOfDoneCatt++;
                        break;

                    case "cities":

                        membersstr = Cut(config, length);
                        CityMembers = FindMembers(membersstr);
                        ConfigList.Add(CityMembers);
                        numberOfDoneCatt++;
                        break;

                    case "status":
                        membersstr = Cut(config, length, true);
                        StatusMembers = FindMembers(membersstr);
                        ConfigList.Add(StatusMembers);
                        numberOfDoneCatt++;
                        break;

                    default:

                        break;
                }

            }
            return ConfigList;
        }

        private List<string> FindMembers(string memberstr)
        {
            List<string> members = new List<string>();
            string member = "";

            for (int i = 0; i < memberstr.Length; i++)
            {
                if (memberstr[i] == ';')
                {
                    members.Add(member);
                    member = "";
                }
                else
                {
                    member += memberstr[i];
                }

            }

            return members;
        }

        private string Cut(string config, int length, bool last = false)
        {
            if (!last)
            {
                config = config.Substring(length + 1);
                int indexsym = 0;
                int i = 0;
                for (i = config.Length - 1; i >= 0; i--)
                    if (config[i] == ':') indexsym = i;

                config = config.Substring(0, indexsym);
                i = config.Length;

                while (config[i - 1] != ';')
                {
                    config = config.Remove(config.Length - 1);
                    i--;
                }
            }
            else
            {
                config = config.Substring(length + 1);
            }
            return config;
        }
    }
}
