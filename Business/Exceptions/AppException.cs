﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Exceptions
{
    public class AppException:Exception
    {
        public AppException(string message) : base(message) { }
    }
}
