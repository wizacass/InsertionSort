using System;
using System.Text;

namespace Lab1.Code
{
    public class ConsoleLogger
    {
        private readonly int[] _spaces;
        private readonly Tuple<string, bool>[] _header;

        public ConsoleLogger(string[] header)
        {
            _header = PersistHeader(header);
            _spaces = new int[header.Length];
            AllocateSpaces();
        }

        public void WriteHeader()
        {
            Console.WriteLine(LineSeparator());
            WriteEntry(_header);
        }

        public void WriteEntry(Tuple<string, bool>[] data)
        {
            var sb = new StringBuilder();

            sb.Append("|");
            for (int i = 0; i < data.Length; i++)
            {
                int width = data[i].Item2 ? _spaces[i] : -_spaces[i];
                string fmt = $"{{0,{width}}}";
                sb.Append($" {string.Format(fmt, data[i].Item1)} |");
            }

            Console.WriteLine(sb.ToString());
            Console.WriteLine(LineSeparator());
        }

        private static Tuple<string, bool>[] PersistHeader(string[] header)
        {
            var headerTuple = new Tuple<string, bool>[header.Length];

            for (int i = 0; i < header.Length; i++)
            {
                headerTuple[i] = new Tuple<string, bool>($" {header[i]} ", false);
            }

            return headerTuple;
        }

        private void AllocateSpaces()
        {
            for (int i = 0; i < _header.Length; i++)
            {
                _spaces[i] = +_header[i].Item1.Length;
            }
        }

        private string LineSeparator()
        {
            var sb = new StringBuilder();

            sb.Append("+");
            foreach (int space in _spaces)
            {
                sb.Append("-");
                sb.Append(new string('-', space));
                sb.Append("-+");
            }

            return sb.ToString();
        }
    }
}
