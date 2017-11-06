using System.Collections.Generic;

namespace Pamux.GameDev.Lib.Interfaces
{
    public interface IUnityPackageProxy
    {
        void EnsureUnpacked();
        void ExtractMetaData(IList<string> assets);

        string UnpackedContentDirectory { get; }
    }
}
