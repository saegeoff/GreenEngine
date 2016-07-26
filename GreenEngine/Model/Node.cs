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
	public class Node
	{
		public Node ()
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
	}
}

