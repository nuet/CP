﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using LLibrary; 

namespace CPiao.TaskRun
{
   
    public class ParameterJob : IJob
    {
        public string Parameter { get; set; }

        static ParameterJob()
        {
            L.Register("[parameter]", "Just executed with parameter \"{0}\".");
        }

        public void Execute()
        {
            L.Log("[parameter]", Parameter);
        }
    }
}