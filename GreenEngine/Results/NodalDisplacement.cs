using System;

namespace GreenEngine.Results
{
    public class NodalDisplacement
    {
        public NodalDisplacement()
        {
            NodeId = -1;
            Degree = DegreeType.X;
            Displacement = 0.0;
        }

        public int NodeId { get; set; }
        public DegreeType Degree { get; set; }
        public double Displacement { get; set; }
    }
}

