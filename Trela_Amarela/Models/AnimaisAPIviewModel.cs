using System.ComponentModel.DataAnnotations;

namespace Trela_Amarela.Models
{
    public class AnimaisAPIviewModel
    {

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
        [Display(Name = "Porte do Animal")]
        public string Porte { get; set; }


        /// Raça do Animal
        /// </summary>
        [Required(ErrorMessage = "A raça é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "A raça não pode ter mais de 60 caracteres.")]
        [Display(Name = "Raça")]
        public string Raca { get; set; }


        /// Vacinação do Animal
        /// </summary>
        [Required(ErrorMessage = "A vacinação é de preenchimento obrigatório")]
        [Display(Name = "Vacinação")]
        public string Vacinacao { get; set; }


        /// Desparasitação do Animal
        /// </summary>
        [Required(ErrorMessage = "A desparasitação é de preenchimento obrigatório")]
        [Display(Name = "Desparasitação")]
        public string Desparasitacao { get; set; }


        /// Necessidades especiais do Animal
        /// </summary>
        [Required(ErrorMessage = "As necessidades especiais são de preenchimento obrigatório")]
        [Display(Name = "Necessidades Especiais")]
        public string N_Especiais { get; set; }


        /// Numero de registo do Animal
        /// </summary>
        [Required(ErrorMessage = "O número de registo é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "O número de registo não pode ter mais de 60 caracteres.")]
        [Display(Name = "Número de Registo")]
        public string Nr_registo { get; set; }



        /// <summary>
        /// Numero de chip do Animal
        /// </summary>
        [Required(ErrorMessage = "O número de chip é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "O número de chip não pode ter mais de 60 caracteres.")]
        [Display(Name = "Número de Chip")]
        public string Nr_chip { get; set; }

        /// <summary>
        /// Foto do Animal
        /// </summary>
        public string Foto { get; set; }



    }
}
