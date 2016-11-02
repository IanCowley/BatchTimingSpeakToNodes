using System;

namespace BatchTimingSpeakToNodes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter the number of targeted dedicated nodes");
            string targetDedicatedNodes = Console.ReadLine();

            var nodeTimer = new TimedNodeCounter(
                "ictest1",
                "hglagKdcEnPysWVy4MlM7UBoXQ6RJaS5VG/38uLrclb7gzyQXBiKNLq4KFU4+PzrCogWPaWO5eTihn97Wm5RSQ==",
                "ListNodesTest",
                int.Parse(targetDedicatedNodes));

            nodeTimer.Start();
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            Console.WriteLine("Once the nodes have finished resizing, press any key to start timing node listing");
            Console.ReadKey();
            nodeTimer.StartListing();
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            nodeTimer.ScaleDown();
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
