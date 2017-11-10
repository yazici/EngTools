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

        string FileName { get; set; }
        string Name { get; set; }
        string RelativePath { get; set; }

        string Guid { get; set; }

        bool IsExpanded { get; set; }

        bool IsLeaf { get; }

        bool IsHarvestable { get; }
        bool HasDependencies { get; }
    }
}
