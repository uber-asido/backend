﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Module.File.EFCore
{
    public class ConnectionString
    {
        public readonly string Value;

        public ConnectionString(string value)
        {
            Value = value;
        }
    }
}
