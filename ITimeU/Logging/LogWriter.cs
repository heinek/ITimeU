using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.CompilerServices;

namespace ITimeU.Logging
{
    public class LogWriter
    {
        private static LogWriter instance = null;
        
        private string file;

        public LogWriter()
        {
            string path = System.IO.Path.GetTempPath() + @"\";
            this.file = path + "Log.txt";
        }

        public LogWriter(string file)
        {
            this.file = file;
        }

        public static LogWriter getInstance() {
            if (instance == null)
                instance = new LogWriter();

            return instance;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Write(string content)
        {
            StreamWriter writer = File.AppendText(file);
            writer.Write(content + "\n");
            writer.Close();
        }
    }
}