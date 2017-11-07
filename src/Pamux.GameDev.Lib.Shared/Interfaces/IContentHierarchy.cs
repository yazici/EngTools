using System;
using System.Collections.Generic;
using System.Text;

namespace Pamux.GameDev.Lib.Interfaces
{
    public interface IContentHierarchy
    {
        IContentHierarchy Root { get; set; }
        IContentHierarchy Parent { get; set; }
        IList<IContentHierarchy> Children { get; set; }

        int Depth { get; set; }

        IContentHierarchy EnsureChild(string name);

        string Name { get; set; }
        string RelativePath { get; set; }

        

        bool IsExpanded { get; set; }

        bool IsHarvestable { get; }
        bool HasDependencies { get; }
    }
}
