﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data
{
    public class StatusStatisticsView
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public int claimCount { get; set; }
    }
}
