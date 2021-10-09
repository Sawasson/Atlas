using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace Jawabkom_Generator3.Core
{
    public static class FileHelper
    {
        public static string[] ReadAllLinesFromUrl(string url, string filename)
        {
            var wreq = WebRequest.Create(url);

            ((HttpWebRequest)wreq).ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            wreq.Timeout = Int32.MaxValue;

            var wresp = (HttpWebResponse)wreq.GetResponse();

            using (Stream file = File.OpenWrite(filename))
            {
                wresp.GetResponseStream().CopyTo(file);
            }

            var subscriptions = System.IO.File.ReadAllLines(filename);

            return subscriptions;
        }

        public static void CreateDirectory(string name, bool deleteExist)
        {
            if (deleteExist)
            {
                if (System.IO.Directory.Exists(name))
                {
                    System.IO.Directory.Delete(name, true);
                }
            }

            System.IO.Directory.CreateDirectory(name);
        }
    }
}
