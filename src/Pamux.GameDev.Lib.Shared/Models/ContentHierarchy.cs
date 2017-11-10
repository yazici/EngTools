using Pamux.GameDev.Lib.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Pamux.GameDev.Lib.Models
{
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ContentHierarchy : IContentHierarchy, INotifyPropertyChanged
    {
        public ContentHierarchy(IContentHierarchy parent, string fileName)
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
            FileName = fileName;
            IsExpanded = Depth <= 3;
            Children = new List<IContentHierarchy>();
        }

        public IContentHierarchy Root { get; set; }
        public int Depth { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }




        public virtual string PreviewImage { get; set; }
        
        private string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                if (value == fileName)
                {
                    return;
                }
                fileName = value;
                Name = Path.GetFileNameWithoutExtension(fileName);
                IsPrefab = fileName.EndsWith(".prefab");
            }
        }
        public string Name { get; set; }
        public string Guid { get; set; }
        public bool IsLeaf { get { return Children.Count == 0; } }
        public string RelativePath { get; set; }

        public bool IsExpanded { get; set; }
        public bool IsPrefab { get; set; }
        public bool IsHarvestable => IsLeaf;

        public bool HasDependencies { get { return IsPrefab; } }

        public void Copy(string sourceRoot, string destinationRoot)
        {
            Copy($"{sourceRoot}\\{RelativePath}", $"{destinationRoot}\\{RelativePath}");
        }

        public IContentHierarchy Parent { get; set; }
        public IList<IContentHierarchy> Children { get; set; }

        internal static IContentHierarchy Add(IContentHierarchy packageRoot, string assetPath)
        {
            var node = packageRoot;
            if (node == null)
            {
                return null;
            }

            var parts = assetPath.Split('/');

            for (int i = 0; i < parts.Length; ++i)
            {
                node = node.EnsureChild(parts[i]);
            }

            node.RelativePath = assetPath;

            return node;
        }

        protected virtual IContentHierarchy CreateChild(string name)
        {
            return new ContentHierarchy(this, name);
        }

        public IContentHierarchy EnsureChild(string fileName)
        {
            foreach (var child in Children)
            {
                if (child.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return child;
                }
            }

            return CreateChild(fileName);
        }
    }
}
