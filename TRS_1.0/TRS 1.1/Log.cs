using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace TRS_1._1
{
    public class Log
    {
        public int adminNo;
        public int usersNo;

        private string m_exePath = string.Empty;
        public Log(string logMessage)
        {
            LogWrite(logMessage);            

        }

        public Log(int adminNo)
        {
            this.adminNo = adminNo;
        }


        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Logger(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
            }
        }

        public void Logger(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("Admin No {0}", adminNo);
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
            }
        }
    }
}
