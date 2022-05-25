using System.ComponentModel.DataAnnotations;

namespace Trela_Amarela.Models
{
    public class Boxs
    {
        /// <summary>
        /// Identificador da Box
        /// </summary>
        [Key]
        public int IdBox { get; set; }


        /// <summary>
        /// Nome da Box
        /// </summary>

        public string Nome { get; set; }

        /// <summary>
        /// Dimensoes da Box
        /// </summary>
        public string Dim_Box { get; set; }
    }
}
