/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *******************************************************/
using System;

namespace GreenEngine.Results
{
    public class ElementAction
    {
        public ElementAction()
        {
            ElementId = -1;
            Stress = 0.0;
        }

        public int ElementId { get; set; }
        public double Stress { get; set; }

        public override string ToString()
        {
            return string.Format("ElementStress: ElementId={0}, Stress={1}", ElementId, Stress);
        }
    }
}

