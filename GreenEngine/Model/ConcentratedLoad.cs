using System;

namespace GreenEngine.Model
{
    public class ConcentratedLoad : Load
    {
        public ConcentratedLoad()
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

