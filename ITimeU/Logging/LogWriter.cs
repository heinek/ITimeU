using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ITimeU.Logging
{
    public class LogWriter
    {
        private static LogWriter instance = null;
        
        private string file;

        public LogWriter()
        {
            this.file = "Log.txt";
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

        public void Write(string content)
        {
            StreamWriter writer = new StreamWriter(file);
            writer.Write(content + "\n");
            writer.Close();
        }
    }
}