/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
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
