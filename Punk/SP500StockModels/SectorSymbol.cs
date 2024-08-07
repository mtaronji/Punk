using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punk.SP500StockModels
{
    public partial class SectorSymbol
    {
        public string Id { get; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<SectorComponent>? Components { get; }

    }
}
