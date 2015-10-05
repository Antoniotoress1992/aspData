using System;

namespace IgniteUI.SamplesBrowser.Models.Repositories
{

    [Flags]
    public enum IncludeChildren
    {
        None = 0,
        Products = 1,
        Employees = 2,
        Orders = 3
    }
}