using Pamux.GameDev.Lib.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Pamux.GameDev.Lib.Models
{
    using System.Linq;

    public class ContentHierarchy : IContentHierarchy
    {
        protected ContentHierarchy() : this(null, "Unity Package Contents")
        {
        }
        public ContentHierarchy(IContentHierarchy parent, string name)
        {
            Parent = parent;

            if (Parent == null)
            {
                Root = this;
                Depth = 1;
            }
            else
            {
                Root = Parent.Root;
                Depth = Parent.Depth + 1;
                Parent.Children.Add(this);
            }
            Name = name;
            IsExpanded = Depth <= 3;
            Children = new List<IContentHierarchy>();
        }

        public IContentHierarchy Root { get; set; }
        public int Depth { get; set; }
        public string Name { get; set; }
        public string RelativePath { get; set; }

        public bool IsExpanded { get; set; }
        public bool IsHarvestable { get { return Children.Count == 0; } }

        public bool HasDependencies { get { return Name.EndsWith("prefab"); } }

        public void Copy(string sourceRoot, string destinationRoot)
        {
            Copy($"{sourceRoot}\\{RelativePath}", $"{destinationRoot}\\{RelativePath}");
        }

        public IContentHierarchy Parent { get; set; }
        public IList<IContentHierarchy> Children { get; set; }

        internal static void Add(IContentHierarchy packageRoot, string assetPath)
        {
            var node = packageRoot;
            if (node == null)
            {
                return;
            }

            var parts = assetPath.Split('/');

            for (int i = 0; i < parts.Length; ++i)
            {
                node = node.EnsureChild(parts[i]);
            }

            node.RelativePath = assetPath;
        }

        protected virtual IContentHierarchy CreateChild(string name)
        {
            return new ContentHierarchy(this, name);
        }

        public IContentHierarchy EnsureChild(string name)
        {
            foreach (var child in Children)
            {
                if (child.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return child;
                }
            }

            return CreateChild(name);
        }
    }
}
