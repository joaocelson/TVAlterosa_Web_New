using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Campeonato.Dominio.contrato;
using Campeonato.Dominio;
using Campeonato.RepositorioADO;

namespace Campeonato.Aplicacao
{
    public class UsuarioAplicacao
    {
        private readonly IRepositorio<Usuario> repositorio;

        public UsuarioAplicacao(IRepositorio<Usuario> repo)
        {
            repositorio = repo;
        }

        public void Salvar(Usuario usuario)
        {
            repositorio.Salvar(usuario);
        }

        public void Excluir(Usuario usuario)
        {
            repositorio.Excluir(usuario);
        }

        public IEnumerable<Usuario> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Usuario ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }

        public List<Usuario> ConverterListausuario()
        {

            return null;

        }

        public Usuario ValidarUsuario(Usuario usuario){
            UsuarioRepositorioADO usuarioADO = new UsuarioRepositorioADO();
            return  usuarioADO.ValidarUsuario(usuario);

        }

        public Usuario ValidarUsuarioEmail(Usuario usuario)
        {
            UsuarioRepositorioADO usuarioADO = new UsuarioRepositorioADO();
            return usuarioADO.ValidarUsuarioEmail(usuario);
        }
    }
}
