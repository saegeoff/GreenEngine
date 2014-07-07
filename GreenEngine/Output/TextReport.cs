using System;
using GreenEngine.Results;
using System.IO;

namespace GreenEngine.Output
{
    public class TextReport
    {
        public TextReport(AnalysisResults results, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine("                              Support Reactions");
                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine("NodeId Fx Fy Fz Mx My Mz");

                foreach (SupportReaction support in results.SupportReactions)
                {
                    writer.WriteLine("{0} {1} {2} {3} {4} {5} {6}", support.NodeId, 
                        support.Fx, support.Fy, support.Fz,
                        support.Mx, support.My, support.Mz);
                }

                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();





                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine("                              Element Actions");
                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine("ElementId NodeIdFx Fy Fz Mx My Mz");

                foreach (SupportReaction support in results.SupportReactions)
                {
                    writer.WriteLine("{0} {1} {2} {3} {4} {5} {6}", support.NodeId, 
                        support.Fx, support.Fy, support.Fz,
                        support.Mx, support.My, support.Mz);
                }

                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();


            }
        }
    }
}

