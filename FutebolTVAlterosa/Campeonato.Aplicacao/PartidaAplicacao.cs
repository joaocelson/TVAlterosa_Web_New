using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Campeonato.Dominio;
using Campeonato.Dominio.contrato;
using Campeonato.Dominio.util;
using Campeonato.RepositorioADO;

namespace Campeonato.Aplicacao
{
    public class PartidaAplicacao
    {
        private readonly IRepositorio<Partida> repositorio;

        public PartidaAplicacao(IRepositorio<Partida> repo)
        {
            repositorio = repo;
        }

        public PartidaAplicacao()
        {            
        }

        public void Salvar(Partida Partida)
        {
            repositorio.Salvar(Partida);
        }

        public void Excluir(Partida Partida)
        {
            repositorio.Excluir(Partida);
        }

        public IEnumerable<Partida> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Partida ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }

        public void GerarPartidasAutomaticamente(String idCampeonato)
        {
            try
            {
                IEnumerable<Time> listaTimes = TimeAplicacaoConstrutor.TimeAplicacaoADO().ListarTimesCampeonato(idCampeonato);

                //GerarPartidasAutomaticamente(listaTimes, idusuarioSelecionado);

                PartidaRepositorioADO partida = new PartidaRepositorioADO();
                partida.GerarPartidaAutomaticamenteTime(listaTimes.ToList<Time>(), idCampeonato);

            }
            catch (Exception ex)
            {
                TratamentoLog.GravarLog("PartidaAplicacao::GerarPartidasAutomaticamente:.. Erro ao geras as partidas automaticamente." + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }


        public void InverterManadate(ref Partida partida)
        {
            try
            {
                String auxTimeMandante = partida.IdTimeMandante;
                partida.IdTimeMandante = partida.IdTimeVisitante;
                partida.IdTimeVisitante = auxTimeMandante;

            }
            catch (Exception ex)
            {

                TratamentoLog.GravarLog("PartidaAplicacao::InverterManadate:. Erro ao inverter o time mandante" + ex.Message, TratamentoLog.NivelLog.Erro);
            }
        }

        public void Resultado(Partida partida)
        {
            PartidaRepositorioADO partidaADO = new PartidaRepositorioADO();
            partidaADO.AtualizarResultado(partida);
        }

        public IEnumerable<Partida> ListaTabelaPorCampeonato(string id)
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return partida.ListaTabelaPorCampeonato(id);
        }

        public object ListarUltimaRodada()
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return partida.ListarUltimaRodada();
        }

        public object ListarProximaRodada()
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return partida.ListarProximaRodada();
        }

        public IEnumerable<Partida> ListarProximaRodadaJson()
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return (IEnumerable<Partida>) partida.ListarProximaRodada();
        }

        public IEnumerable<Partida> ListarProximaRodadaPorBolao(string id)
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return partida.ListarProximaRodadaPorBolao(id);
        }

        public void ComentarPartida(string id, string comentario)
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            partida.ComentarPartida(id, comentario);

        }
        public IEnumerable<JogoOnline> ComentarioPartida(string id)
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return partida.ComentarioPartida(id);
        }

        public IEnumerable<Partida> ListarPartidasPorData(string data)
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return partida.ListarPartidasPorData(data);
        }

        public IEnumerable<Partida> ListaTabelaPorCampeonatoSegundaDivisao(string id)
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return partida.ListaTabelaPorCampeonatoSegundaDivisao(id);
        }


        public List<AoVivo> PartidaAoVivo(string id)
        {
            PartidaRepositorioADO partida = new PartidaRepositorioADO();
            return partida.PartidaAoVivo(id);
        }

        
    }
}
