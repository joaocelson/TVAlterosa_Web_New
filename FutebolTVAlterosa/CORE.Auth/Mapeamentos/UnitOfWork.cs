using System;
using CORE.Auth.Contexto;

namespace CORE.Auth.Mapeamentos
{
    /// <summary>
    /// Classe que representa o Padrão UnitOfWork que encapsula os repositórios e o contexto
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        /// <summary>
        /// Campo que representa o contexto(repositório) do banco de dados
        /// </summary>
        public Entities ctx = null;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="isFromTestMethod">Controle que indica se a classe está sendo criada dentro de um test method</param>
        public UnitOfWork()
        {
            ctx = new Entities();

        }

       

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ctx.Dispose();
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        #endregion
    }
}
