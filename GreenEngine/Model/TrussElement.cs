/*******************************************************
 * Copyright (C) 2014 Geoffrey Trees gptrees@gmail.com
 * 
 * This file is part of GreenEngine.
 * 
 * GreenEngine can not be copied and/or distributed without the express
 * permission of Geoffrey Trees
 *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenEngine.Model
{
    public class TrussElement : Element
    {
        public TrussElement()
        {
            Area = 0.0;
        }

        public Node Node1 { get; set; }
        public Node Node2 { get; set; }

        public Material Material { get; set; }

        public double Area { get; set; }
    }
}
