using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChallenge.Domain;

namespace VRChallenge.DB.Model
{
    public class Box
    {
        public Guid Id { get; set; }
        public string SupplierIdentifier { get; set; }
        public string Identifier { get; set; }
        public List<Item> Items { get; set; }
    }
}
