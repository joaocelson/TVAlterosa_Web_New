using System.IO;
using System.Linq;
using System.Reflection;

namespace CORE.Componentes.Personalizados.AutoComplete.Helpers
{
    /// <summary>
    /// Classe responsável pela manipulação de arquivos de recurso do sistema
    /// </summary>
    public static class ResourcesHelper
    {
        /// <summary>
        /// Método que retorna um recurso como string
        /// </summary>
        /// <param name="name">Nome do recurso</param>
        /// <returns>Recurso convertido em string</returns>
        public static string LoadResourceAsString(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var fullResourceName = assembly.GetManifestResourceNames().Where(i => i.Contains(name)).FirstOrDefault();
                
            string resourceString = "";

            using (Stream stream = assembly.GetManifestResourceStream(fullResourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                resourceString = reader.ReadToEnd();
            }

            return resourceString;
        }
    }
}