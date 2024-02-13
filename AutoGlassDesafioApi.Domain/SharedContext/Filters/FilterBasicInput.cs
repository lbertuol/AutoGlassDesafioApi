using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.SharedContext.Filters
{    
    public abstract class FilterBasicInput
    {
        public int Pagina { get; set; }
        public int PorPagina { get; set; }
        public string CampoFiltro { get; set; }
        public string ValorFiltro { get; set; }        
    }
}
