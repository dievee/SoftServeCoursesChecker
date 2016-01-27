using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using CsQuery;

namespace SSCoursesChecker
{
    public class StreamData
    {

        public string Read()
        {
            WebClient httpClient = new WebClient();
            Stream data = httpClient.OpenRead("https://softserve.ua/app/forms/itacademyschedule/EN/");
            StreamReader readStream = new StreamReader(data);
            string s = readStream.ReadToEnd();

            return s;
        }

        public List<string> Parse(string type)
        {
            List<string> Arr = new List<string>();
            CQ cq = CQ.Create(Read());
            ConfigRead cr = new ConfigRead();
            switch (type)
            {
                case "link":

                    foreach (IDomObject obj in cq.Find("div.clearfix h2.card-courses_title a "))
                    {
                        Arr.Add(obj.GetAttribute("href"));
                    }
                    break;

                case "technology":

                    foreach (IDomObject obj in cq.Find("div.row dl.col-xs-6:nth-child(2) dd"))
                    {
                        Arr.Add(WebUtility.HtmlDecode( cr.DeleteExcess(obj.InnerText) ));
                        
                    }
                    break;

                case "city":

                    foreach (IDomObject obj in cq.Find("div.row dl.col-xs-6:nth-child(3) dd"))
                    {
                        Arr.Add(WebUtility.HtmlDecode(cr.DeleteExcess(obj.InnerText) ));
                    }
                    break;

                case "status":

                    foreach (IDomObject obj in cq.Find("div.card-courses_label .label"))
                    {
                        Arr.Add(WebUtility.HtmlDecode(cr.DeleteExcess(obj.InnerText) ));
                    }
                    break;
                default:

                    Arr.Add("-1");
                    break;
            }

            return Arr;
        }

       
       
    }
}
