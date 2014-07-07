using System;
using GreenEngine.Model;
using GreenEngine;
using GreenEngine.Results;
using GreenEngineConsole.Tests;
using System.Collections.Generic;

namespace GreenEngineConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            RunAllTests();

            Console.ReadKey();
		}

        protected static void RunAllTests()
        {
            List<Test> testList = new List<Test>();

            //testList.Add(new TrussTest1());  // Test not tested yet
            testList.Add(new TrussTest2());
            testList.Add(new TrussTest3());
            testList.Add(new BeamTest1());
            testList.Add(new BeamTest2());

            foreach (Test test in testList)
            {
                Console.Write(test.Name + ": ");

                bool bTest = test.Run();

                if (bTest)
                    Console.WriteLine("Passed...");
                else
                    Console.WriteLine("Failed...");
            }
        }
	}
}
