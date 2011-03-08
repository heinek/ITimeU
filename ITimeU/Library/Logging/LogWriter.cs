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

        /// <summary>
        /// The file used by this log writer.
        /// </summary>
        public string LogFile { get; private set; }

        public LogWriter()
        {
            string path = System.IO.Path.GetTempPath() + @"\";
            LogFile = path + "Log.txt";
        }

        public LogWriter(string file)
        {
            LogFile = file;
        }

        public static LogWriter getInstance() {
            if (instance == null)
                instance = new LogWriter();

            return instance;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Write(string content)
        {
            StreamWriter writer = File.AppendText(LogFile);
            writer.Write(content + "\n");
            writer.Close();
        }
    }
}