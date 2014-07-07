using System;

namespace GreenEngine.Model
{
	public class BeamElement : Element
	{
		public BeamElement ()
		{
            MomentOfInertia = 0.0;
		}

        public Node Node1 { get; set; }
        public Node Node2 { get; set; }

        public Material Material { get; set; }

        public double MomentOfInertia { get; set; }
	}
}

