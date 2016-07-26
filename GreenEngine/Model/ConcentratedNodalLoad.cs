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

