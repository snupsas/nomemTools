using System;
using System.Text;
using System.IO;

namespace nomemTools
{
    static class ErrorLoger
    {
        public static void Log(Exception ex)
        {
            var fileName = "ErrorLog.txt";
            var path = AppDomain.CurrentDomain.BaseDirectory;

            if (!File.Exists(path + "\\" + fileName))
            {
                using (var fileStream = new FileStream(path + "\\" + fileName, FileMode.Create, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.Write(MsgFormat(ex));
                    }
                }
            }
            else
            {
                using (var fileStream = new FileStream(path + "\\" + fileName, FileMode.Append, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.Write(MsgFormat(ex));
                    }
                }
            }   
        }


        private static string MsgFormat(Exception ex)
        {
            var tmp = string.Format("{0}{1}{2}{3}{4}{5}",
                                        "---------------------",
                                        Environment.NewLine,
                                        DateTime.Now.ToString(),
                                        Environment.NewLine,
                                        ex.ToString(),
                                        Environment.NewLine);
            return tmp;
        }
    }
}