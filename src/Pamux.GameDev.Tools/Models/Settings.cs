using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.Tools.Models
{
    public class Settings
    {
        public static Settings Instance = new Settings();

        public string VoiceSaveDirectory { get; internal set; }
    }
}
