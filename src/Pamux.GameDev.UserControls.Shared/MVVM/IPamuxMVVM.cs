using System;
using System.Collections.Generic;
using System.Text;

namespace Pamux.GameDev.UserControls.MVVM
{
    public interface IPamuxMVVM
    {
        IPamuxModel M { get; set; }
        IPamuxView V { get; set; }
        IPamuxViewModel VM { get; set; }
    }
}
