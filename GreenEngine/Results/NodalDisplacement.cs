using System;

namespace GreenEngine.Results
{
    public class NodalDisplacement
    {
        public NodalDisplacement()
        {
            NodeId = -1;
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        public int NodeId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override string ToString()
        {
            return string.Format("NodalDisplacement: NodeId={0}, X={1}, Y={2}, Z={3}", NodeId, X, Y, Z);
        }
    }
}

