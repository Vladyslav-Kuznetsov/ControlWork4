using System;
using System.IO;
using System.Threading.Tasks;

namespace ControlWork4
{
    public class FileWatcher
    {
        public event Action Change;
        private object _loker;
        private string _path;
        private DateTime _lastWriteTime;

        public FileWatcher(string path, object loker)
        {
            _path = path;
            _loker = loker;
            _lastWriteTime = File.GetLastWriteTime(_path);
        }

        public void Start()
        {
            DateTime lastWriteTime;

            while (true)
            {
                lock (_loker)
                {
                    lastWriteTime = File.GetLastWriteTime(_path);
                }

                if (_lastWriteTime != lastWriteTime)
                {
                    _lastWriteTime = File.GetLastWriteTime(_path);
                    Task.Run(() => Change?.Invoke());
                }
            }
        }
    }
}
