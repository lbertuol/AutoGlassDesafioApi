using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.SharedContext.DTO
{
    public abstract class EntityBasic
    {
        public EntityBasic()
        {
            this.DataCriacao = DateTime.Now;
        }

        public EntityBasic(int Id)
        {

        }

        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
