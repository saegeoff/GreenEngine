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
                writer.WriteLine("ElementId Stress");

                foreach (ElementAction elementAction in results.ElementActions)
                {
                    writer.WriteLine("{0} {1}", elementAction.ElementId, 
                        elementAction.Stress);
                }

                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();



                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine("                              Displacements");
                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine("NodeId Tx Ty Tz Rx Ry Rz");

                foreach (NodalDisplacement disp in results.NodalDisplacements)
                    {
                    writer.WriteLine("{0} {1} {2} {3} {4} {5} {6}", disp.NodeId, 
                        disp.Tx, disp.Ty, disp.Tz,
                        disp.Rx, disp.Ry, disp.Rz);
                    }

                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();


            }
        }
    }
}

