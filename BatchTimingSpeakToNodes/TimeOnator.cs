using System;
using System.Diagnostics;

namespace BatchTimingSpeakToNodes
{
    public class TimeOnator
    {
        readonly Stopwatch _stopwatch;

        public TimeOnator()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void WriteTimedNote(string note)
        {
            Console.WriteLine($"{_stopwatch.Elapsed.TotalSeconds.ToString("F2")} > {note}");
        }
    }
}
