﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Core.Setup
{
    public class InstallerException : Exception
    {
        public InstallerException(IList<string> errors)
            : base(FormatMessage(errors)) { }

        private static string FormatMessage(IList<string> errors)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Installer failed with the following {errors.Count} errors:");

            foreach (var error in errors)
                builder.AppendLine("  • " + error);

            return builder.ToString();
        }
    }
}
