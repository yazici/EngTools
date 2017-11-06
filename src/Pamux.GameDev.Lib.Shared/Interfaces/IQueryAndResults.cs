using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Pamux.GameDev.Lib.Interfaces
{
    public interface IQueryAndResults
    {

    }

    public interface IQueryAndResults<T> : IQueryAndResults
    {
        string Query { get; }
        IEnumerable<T> Results { get; }

        DataTemplateSelector RowDetailsTemplateSelector { get; }
}
}
