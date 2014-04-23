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

        public double Load
        {
            get;
            set;
        }

        public double X;
        public double Y;
        public double Z;
    }
}

