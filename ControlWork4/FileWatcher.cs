using System;
using System.IO;
using System.Threading.Tasks;

namespace ControlWork4
{
    public class FileWatcher
    {
        public event Action<DateTime> Change;
        private string _path;
        private DateTime _lastWriteTime;

        public FileWatcher(string path)
        {
            _path = path;
            _lastWriteTime = File.GetLastWriteTime(_path);
        }

        public void Start()
        {
            DateTime lastWriteTime;

            while (true)
            {
                lastWriteTime = File.GetLastWriteTime(_path);

                if (_lastWriteTime != lastWriteTime)
                {
                    _lastWriteTime = File.GetLastWriteTime(_path);
                    Task.Run(() => Change?.Invoke(_lastWriteTime));
                }
            }
        }
    }
}
