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

