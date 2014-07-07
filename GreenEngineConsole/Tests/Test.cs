using System;
using GreenEngine.Model;
using GreenEngine.Results;

namespace GreenEngineConsole.Tests
{
    public abstract class Test
    {
        protected FiniteElementModel m_Model;
        protected AnalysisResults m_Results;
        protected string m_TestName = string.Empty;

        public Test()
        {
        }

        public string Name
        {
            get { return m_TestName; }
        }

        public abstract bool Run();

        public void OutputResults()
        {
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Node Displacements:");
            foreach (NodalDisplacement disp in m_Results.NodalDisplacements)
            {
                Console.WriteLine(disp);
            }
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Element Stresses:");
            foreach (ElementStress stress in m_Results.ElementStresses)
            {
                Console.WriteLine(stress);
            }
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Support Reactions:");
            foreach (SupportReaction supportReaction in m_Results.SupportReactions)
            {
                Console.WriteLine(supportReaction);
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        protected bool IsEqualTo(double d1, double d2, double dTol = 0.00001)
        {
            return Math.Abs(d1 - d2) < dTol;
        }
    }
}

