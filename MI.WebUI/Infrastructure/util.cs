using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MI.WebUI.Infrastructure
{
    public static  class util
    {

        public static void dodebug(String s)
        {

            if (s.Trim() != "")
            {
                string path = @"C:\debug.txt";
                StreamWriter SW = new StreamWriter(path, true);
                SW.WriteLine(s);
                SW.Flush();
                SW.Close();
            }
        }
        
    }
}