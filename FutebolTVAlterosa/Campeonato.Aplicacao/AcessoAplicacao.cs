using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.RepositorioADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.Aplicacao
{
    public class AcessoAplicacao
    {
        private readonly IRepositorio<Acesso> repositorio;

        public AcessoAplicacao(IRepositorio<Acesso> repo)
        {
            repositorio = repo;
        }
        public void AtualizarNumeroAcesso()
        {
            try
            {
                AcessoRepositorioADO acesso = new AcessoRepositorioADO();
                acesso.AtualizarNumeroAcesso();
            }
            catch (Exception ex)
            {
            }
        }

        public void NumeroAcesso()
        {
            try
            {
                AcessoRepositorioADO acesso = new AcessoRepositorioADO();
                acesso.NumeroAcesso();
            }
            catch (Exception ex)
            {
            }
        }
        
    }
}
