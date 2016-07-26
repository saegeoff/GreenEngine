/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
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

