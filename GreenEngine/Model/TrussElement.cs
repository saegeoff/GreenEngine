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
