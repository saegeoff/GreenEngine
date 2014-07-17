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

