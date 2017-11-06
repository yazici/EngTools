using System;
using System.Collections.Generic;
using System.Text;

namespace Pamux.GameDev.Lib.Interfaces
{
    public interface IContentHierarchy
    {
        IContentHierarchy Parent { get; set; }
        IList<IContentHierarchy> Children { get; set; }

        int Depth { get; set; }

        IContentHierarchy EnsureChild(string name);

        string Name { get; set; }
        bool IsExpanded { get; set; }
    }
}
