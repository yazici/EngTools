using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.Lib.Models
{
    public class AssetQuery
    {
        public string query;
        public List<string> tokens = new List<string>();
        public AssetQuery(string query)
        {
            this.query = query.Trim().ToLower();

            foreach (string token in this.query.Split(UnityPackageMetaData.Separators))
            {
                string t = token.Trim();

                if (t.Length >= 3 || t == "3D")
                {
                    tokens.Add(t);
                }
            }
        }
    }
}
