using System;
using System.IO;

namespace Bergfall.Utils
{
    public sealed class Logger : IDisposable
    {
        private FileStream _fs;
        private StreamWriter _sw;

        private Logger()
        {
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "bergfall.log");
            try
            {
                _fs = new FileStream(logFilePath, FileMode.OpenOrCreate);
                _sw = new StreamWriter(_fs);
            }
            catch (Exception)
            {
            }
        }

        private static Logger _logger;

        public static Logger GetInstance()
        {
            if (_logger == null)
            {
                lock (typeof(Logger))
                {
                    _logger = new Logger();
                }
            }
            return _logger;
        }

        public void Log(Exception exp)
        {
            _sw.WriteLine("EXCEPTION\t" + exp.Message);
            if (exp.InnerException != null)
            {
                _sw.WriteLine(exp.InnerException.Message);
            }
            _sw.WriteLine("--------------------------------------------------------");
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_fs.SafeFileHandle.IsInvalid || _fs.SafeFileHandle.IsClosed || _fs == null)
            {
            }
            else
            {
                _fs.Close();
            }
        }

        #endregion IDisposable Members
    }
}