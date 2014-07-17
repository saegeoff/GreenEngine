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
    public class NodalDisplacement
    {
        public NodalDisplacement()
        {
            NodeId = -1;
            Tx = 0.0;
            Ty = 0.0;
            Tz = 0.0;
            Rx = 0.0;
            Ry = 0.0;
            Rz = 0.0;
        }

        public int NodeId { get; set; }
        public double Tx { get; set; }
        public double Ty { get; set; }
        public double Tz { get; set; }
        public double Rx { get; set; }
        public double Ry { get; set; }
        public double Rz { get; set; }

        public override string ToString()
        {
            return string.Format("NodalDisplacement: NodeId={0}, Tx={1}, Ty={2}, Tz={3}, Rx={4}, Ry={5}, Rz={6}", NodeId, Tx, Ty, Tz, Rx, Ry, Rz);
        }
    }
}

