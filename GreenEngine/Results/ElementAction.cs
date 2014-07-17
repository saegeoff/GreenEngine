/*******************************************************
 * Copyright (C) 2014 Geoffrey Trees gptrees@gmail.com
 * 
 * This file is part of GreenEngine.
 * 
 * GreenEngine can not be copied and/or distributed without the express
 * permission of Geoffrey Trees
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

