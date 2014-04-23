using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenEngine.Model
{
    public class TrussElement : Element
    {

        // TODO: These should be properties... this is quick and dirty
        public Node Node1;
        public Node Node2;

        public Material Material;
        public double Area;
    }
}
