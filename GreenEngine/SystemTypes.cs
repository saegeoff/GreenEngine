/*******************************************************
 * Copyright (C) 2014 Geoffrey Trees gptrees@gmail.com
 * 
 * This file is part of GreenEngine.
 * 
 * GreenEngine can not be copied and/or distributed without the express
 * permission of Geoffrey Trees
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
