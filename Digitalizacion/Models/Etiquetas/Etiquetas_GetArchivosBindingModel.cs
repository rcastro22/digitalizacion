using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Etiquetas
{
    public class Etiquetas_GetArchivosBindingModel
    {
        [Required]
        [MaxLength(2)]
        public string Aplicacion { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public short Categoria { get; set; }

        [Required]
        public IEnumerable<Etiquetas> Etiquetas { get; set; }

        public string QueryString()
        {
            string query = string.Format("?Aplicacion={0}&Categoria={1}", Aplicacion, Categoria);

            for (int i = 0; i < Etiquetas.Count(); i++)
            {
                query += string.Format("&Etiquetas[{0}].Etiqueta={1}&Etiquetas[{0}].Valor={2}", i, Etiquetas.ElementAt(i).Etiqueta, Etiquetas.ElementAt(i).Valor);
            }

            return query;
        }
    }
}
