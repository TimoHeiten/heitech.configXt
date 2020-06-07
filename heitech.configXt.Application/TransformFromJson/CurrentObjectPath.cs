using System.Linq;

namespace heitech.configXt.Application
{
    public class CurrentObjectPath
    {
        private string _current;
        public CurrentObjectPath(string start)
        {
            _current = start;
        }
        public bool Exists => _current != string.Empty;
        public void Add(string nextPath)
        {
            _current += _current == string.Empty
                                  ? nextPath 
                                  : ":" + nextPath;
            
            _current = string.Join(":", _current.Split(':').Distinct());
        }

        public void SubtractOne()
        {
            string[] s = _current.Split(':');
            if (s.Length == 1)
            {
                _current = string.Empty;
            }
            else 
            {
                _current = string.Join(":", s.Take(s.Length - 1));
            }
        }

        public override string ToString()
        {
            return _current;
        }
    }
}