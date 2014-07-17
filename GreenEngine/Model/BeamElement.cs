/*******************************************************
 * Copyright (C) 2014 Geoffrey Trees gptrees@gmail.com
 * 
 * This file is part of GreenEngine.
 * 
 * GreenEngine can not be copied and/or distributed without the express
 * permission of Geoffrey Trees
 *******************************************************/
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

