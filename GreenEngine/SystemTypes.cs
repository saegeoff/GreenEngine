/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *******************************************************/
using System;

namespace GreenEngine
{
    public enum DegreeType
    {
        Fx,
        Fy,
        Fz,
        Mx,
        My,
        Mz,
    }

    public enum TranslationType
    {
        Constrained,
        Released
    }

    public enum RotationType
    {
        Constrained,
        Released
    }
}
