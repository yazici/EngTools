using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.UserControls.Models
{
    public struct WorldSize
    {
        public float X;
        public float Y;
    }
    public struct ViewSize
    {
        public int X;
        public int Y;
    }

    public class GeneratorConfigModel : PamuxModelBase
    {
        public GeneratorConfigModel(IPamuxViewModel vm)
            : base(vm)
        {
        }

        public WorldSize WorldSize = new WorldSize { X = 10000f, Y = 10000f };
        public WorldSize ChunkSize = new WorldSize { X = 100f, Y = 100f };

        public ViewSize ViewSize = new ViewSize { X = 100, Y = 100 };
    }
}
