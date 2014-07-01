using System;

namespace GreenEngine.Model
{
    public class ConcentratedNodalLoad : Load
    {
        public ConcentratedNodalLoad()
        {
        }

        public Node Node
        {
            get;
            set;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}

