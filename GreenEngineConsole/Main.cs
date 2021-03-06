/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *******************************************************/
using System;
using GreenEngine.Model;
using GreenEngine;
using GreenEngine.Results;
using GreenEngineConsole.Tests;
using System.Collections.Generic;
using GreenEngine.Output;

namespace GreenEngineConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            RunAllTests();

            //BeamTest2 test = new BeamTest2();
            //test.Run();
            //test.OutputResults();

            //TextReport report = new TextReport(test.Results, "D:\\output.txt");

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

                //TextReport report = new TextReport(test.Results, string.Format("/home/gtrees/{0}.txt", ++iId));
            }
        }
	}
}
