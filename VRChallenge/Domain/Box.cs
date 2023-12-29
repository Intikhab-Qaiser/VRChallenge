using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChallenge.Domain
{
    public class Box
    {
        public string SupplierIdentifier { get; set; }
        public string Identifier { get; set; }
        public IReadOnlyCollection<Content> Contents { get; set; }
    }
}
