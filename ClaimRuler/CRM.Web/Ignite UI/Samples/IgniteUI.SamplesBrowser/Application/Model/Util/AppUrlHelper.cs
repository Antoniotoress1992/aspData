using Infragistics.IgniteUI.SamplesBrowser.Shared.Contracts;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Util;

namespace IgniteUI.SamplesBrowser.Application.Model
{
    public class AppUrlHelper : AppUrlHelperBase
    {
        public AppUrlHelper(IFileSystem fs, IResourceStrings resources)
            : base(fs, resources) { }
    }
}