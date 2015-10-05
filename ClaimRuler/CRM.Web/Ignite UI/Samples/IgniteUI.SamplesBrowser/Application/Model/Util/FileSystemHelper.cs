using Infragistics.IgniteUI.SamplesBrowser.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IgniteUI.SamplesBrowser.Application.Model.Util;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Contracts;

namespace IgniteUI.SamplesBrowser.Application.Model
{
    public class FileSystemHelper : FileSystemBase
    {
        public FileSystemHelper(IConfig config)
            : base(config) { }
    }
}