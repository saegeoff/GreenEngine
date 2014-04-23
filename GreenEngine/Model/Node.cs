using System;

namespace GreenEngine.Model
{
	public class Node
	{
		public Node ()
		{
			X = 0.0;
			Y = 0.0;
			Z = 0.0;
		}

        public int NodeId
        {
            get;
            set;
        }

		public double X 
		{
			get;
			set;
		}

		public double Y
		{
			get;
			set;
		}

		public double Z 
		{
			get;
			set;
		}

        public bool Fx = false;
        public bool Fy = false;
	}
}

