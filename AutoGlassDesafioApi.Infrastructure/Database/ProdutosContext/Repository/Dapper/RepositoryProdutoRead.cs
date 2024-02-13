using AutoGlassDesafioApi.Domain.ProdutosContext.Filtros;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.Dapper;
using AutoGlassDesafioApi.Domain.ProdutosContext.Models;
using AutoGlassDesafioApi.Infrastructure.Database.SharedContext.Repository.Dapper;
using Microsoft.Extensions.Configuration;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Infrastructure.Database.ProdutosContext.Repository.Dapper
{
    public class RepositoryProdutoRead : RepositoryBaseDapper<ProdutoOutput, FilterProdutoInput>, IRepositoryProdutoRead
    {
        public RepositoryProdutoRead(IConfiguration config) : base(config)
        {
        }

        public override async Task<Option<IEnumerable<ProdutoOutput>>> ListarAsync(FilterProdutoInput filtros)
        {
            List<ProdutoOutput> retorno = new List<ProdutoOutput>();

            return await RetornarTodosAsync(BuscarProdutos(filtros));
        }

        private string BuscarQuantidadeProdutos(FilterProdutoInput filtros)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT Count(Prod.Id) FROM Produtos Prod");            
            sb.AppendLine(Filtrar(filtros, false));

            return sb.ToString();
        }

        private string BuscarProdutos(FilterProdutoInput filtro)
        {
            int avanco = ((filtro.PorPagina * filtro.Pagina) - filtro.PorPagina);

            var sb = new StringBuilder();
            sb.Clear();
            sb.AppendLine("SELECT");
            sb.AppendLine(" Prod.Id,");
            sb.AppendLine(" Prod.Codigo,");
            sb.AppendLine(" Prod.Descricao,");
            sb.AppendLine(" Prod.Situacao,");
            sb.AppendLine(" Prod.DataFabricacao,");
            sb.AppendLine(" Prod.DataValidade,");
            sb.AppendLine(" Prod.FornecedorCodigo,");
            sb.AppendLine(" Prod.FornecedorDescricao,");
            sb.AppendLine(" Prod.FornecedorCNPJ");            
            sb.AppendLine(" FROM Produtos Prod");           

            sb.AppendLine(Filtrar(filtro));

            sb.AppendLine($" OFFSET {avanco} ROWS ");
            sb.AppendLine($" FETCH NEXT {filtro.PorPagina} ROWS ONLY");
            return sb.ToString();
        }       

        private string Filtrar(FilterProdutoInput filtro, bool ordenar = true)
        {
            var sb = new StringBuilder();
            bool ordenarPorCampo = false;

            if (!string.IsNullOrWhiteSpace(filtro.ValorFiltro))
            {
                if ((!string.IsNullOrEmpty(filtro.CampoFiltro)) && (!string.IsNullOrEmpty(filtro.ValorFiltro)))
                {
                    sb.AppendLine($" WHERE {filtro.CampoFiltro} LIKE '%{filtro.ValorFiltro}%'");
                    ordenarPorCampo = true;
                }
            }            

            if (ordenar)
            {
                if (ordenarPorCampo)
                    sb.AppendLine($" ORDER BY {filtro.CampoFiltro}");
                else
                    sb.AppendLine(" ORDER BY Prod.Descricao");
            }

            return sb.ToString();
        }

        private string RetornarIds(List<int> ids)
        {
            string valor = "";
            if (ids.Count > 0)
            {
                valor = "";
                int contador = 1;

                foreach (var item in ids)
                {
                    if (contador == 1)
                        valor = valor + item.ToString();
                    else
                        valor = valor + "," + item.ToString();
                    contador++;
                }
            }
            return valor;
        }
    }
}
