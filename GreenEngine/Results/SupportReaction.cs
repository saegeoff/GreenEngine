using System;

namespace GreenEngine.Results
{
    public class SupportReaction
    {
        public SupportReaction()
        {
            NodeId = -1;

            Fx = 0.0;
            Fy = 0.0;
            Fz = 0.0;

            Mx = 0.0;
            My = 0.0;
            Mz = 0.0;
        }

        public int NodeId { get; set; }

        public double Fx { get; set; }
        public double Fy { get; set; }
        public double Fz { get; set; }

        public double Mx { get; set; }
        public double My { get; set; }
        public double Mz { get; set; }

        public override string ToString()
        {
            return string.Format("SupportReaction: NodeId={0}, Fx={1}, Fy={2}, Fz={3}, Mx={4}, My={5}, Mz={6}", NodeId, Fx, Fy, Fz, Mx, My, Mz);
        }
    }
}

