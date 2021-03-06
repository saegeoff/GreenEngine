﻿/*******************************************************
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
using System.Threading.Tasks;

namespace GreenEngine.Model
{
    public class Support
    {
        public Support()
		{
            Tx = TranslationType.Released;
            Ty = TranslationType.Released;
            Tz = TranslationType.Released;

            Rx = RotationType.Released;
            Ry = RotationType.Released;
            Rz = RotationType.Released;
		}

        public Node Node { get; set; }

        public TranslationType Tx { get; set; }
        public TranslationType Ty { get; set; }
        public TranslationType Tz { get; set; }

        public RotationType Rx { get; set; }
        public RotationType Ry { get; set; }
        public RotationType Rz { get; set; }

        public int GetNumberOfConstraints()
        {
            int i = 0;

            if (Tx == TranslationType.Constrained)
                ++i;

            if (Ty == TranslationType.Constrained)
                ++i;

            if (Tz == TranslationType.Constrained)
                ++i;

            if (Rx == RotationType.Constrained)
                ++i;

            if (Ry == RotationType.Constrained)
                ++i;

            if (Rz == RotationType.Constrained)
                ++i;

            return i;
        }
    }
}
