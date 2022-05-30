using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Trela_Amarela.Models
{
    public class Animais
    {
        public Animais()
        {
            ListaReservas = new HashSet<Reservas>();
        }


        /// <summary>
        /// Identificador do Animal
        /// </summary>
        [Key]
        public int IdAnimal { get; set; }

        /// Nome do Animal
        /// </summary>
        [Required(ErrorMessage = "O Nome é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "O Nome não pode ter mais de 60 caracteres.")]
        public string Nome { get; set; }

        /// <summary>
        /// Data de Nascimento
        /// </summary>
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNasc { get; set; }

        /// Porte do Animal
        /// </summary>
        [Required(ErrorMessage = "O Porte é de preenchimento obrigatório")]
        public string Porte { get; set; }
        [Display(Name = "Porte do Animal")]

        /// Raça do Animal
        /// </summary>
        [Required(ErrorMessage = "A raça é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "A raça não pode ter mais de 60 caracteres.")]
        public string Raca { get; set; }
        [Display(Name = "Raça")]

        /// Vacinação do Animal
        /// </summary>
        [Required(ErrorMessage = "A vacinação é de preenchimento obrigatório")]
        public string Vacinacao { get; set; }
        [Display(Name = "Vacinação")]

        /// Desparasitação do Animal
        /// </summary>
        [Required(ErrorMessage = "A desparasitação é de preenchimento obrigatório")]
        public string Desparasitacao{ get; set; }
        [Display(Name = "Desparasitação")]

        /// Necessidades especiais do Animal
        /// </summary>
        [Required(ErrorMessage = "As necessidades especiais são de preenchimento obrigatório")]
        public string N_Especiais { get; set; }
        [Display(Name = "Necessidades Especiais")]

        /// Numero de registo do Animal
        /// </summary>
        [Required(ErrorMessage = "O número de registo é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "O número de registo não pode ter mais de 60 caracteres.")]
        public string Nr_registo { get; set; }
        [Display(Name = "Número de Registo")]


        /// <summary>
        /// Numero de chip do Animal
        /// </summary>
        [Required(ErrorMessage = "O número de chip é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "O número de chip não pode ter mais de 60 caracteres.")]
        public string Nr_chip { get; set; }
        [Display(Name = "Número de Chip")]
        /// <summary>
        /// Foto do Animal
        /// </summary>
        public string Foto { get; set; }


        //********************************************************************************
        public ICollection<Reservas> ListaReservas { get; set; }

        //********************************************************************************

        /// <summary>
        /// FK para identificar o CLiente
        /// </summary>
        [ForeignKey(nameof(Cliente))]
        public int IdCliente { get; set; }
        public Clientes Cliente { get; set; }

        //********************************************************************************
    }

}