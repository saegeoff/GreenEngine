using System;

namespace GreenEngine.Model
{
	public class Node
	{
		public Node ()
		{
            NodeId = -1;
            
            X = 0.0;
			Y = 0.0;
			Z = 0.0;

            Tx = true;
            Ty = true;
            Tz = true;

            Rx = true;
            Ry = true;
            Rz = true;
		}

        public int NodeId { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public bool Tx { get; set; }
        public bool Ty { get; set; }
        public bool Tz { get; set; }

        public bool Rx { get; set; }
        public bool Ry { get; set; }
        public bool Rz { get; set; }
	}
}

