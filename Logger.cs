using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.DataGrid.Converters;

namespace NIR_WPF
{
    class Logger
    {
        private static string _logPath = "log.txt";
        private static StreamWriter _logWriter = new StreamWriter(_logPath);
        private Type _type;

        private Logger(Type type)
        {
            this._type = type;
        }

        public static Logger GetInstance(Type type)
        {
            return new Logger(type);
        }

        public void Debug(string log)
        {
            _logWriter.WriteLine("[DEBUG]:{0}:{1}", _type.FullName, log);
        }

        public void Info(string log)
        {
            _logWriter.WriteLine("[INFO]:{0}:{1}", _type.FullName, log);
        }

        public void Error(string log)
        {
            _logWriter.WriteLine("[ERROR]:{0}:{1}", _type.FullName, log);
        }
    }
}
