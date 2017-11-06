using Pamux.GameDev.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pamux.GameDev.Lib.Models
{
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
                Depth = 1;
            }
            else
            {
                Depth = Parent.Depth + 1;
                Parent.Children.Add(this);
            }
            Name = name;
            IsExpanded = Depth <= 3;
            Children = new List<IContentHierarchy>();
        }

        public int Depth { get; set; }
        public string Name { get; set; }

        public bool IsExpanded { get; set; }

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
        }

        public IContentHierarchy EnsureChild(string name)
        {
            foreach (var child in Children)
            {
                if (child.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return child as ContentHierarchy;
                }
            }

            return new ContentHierarchy(this, name);
        }
    }
}
