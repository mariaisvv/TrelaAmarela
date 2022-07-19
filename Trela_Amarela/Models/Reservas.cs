using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trela_Amarela.Models
{
    public class Reservas
    {
        public Reservas() {

            ListaAnimais = new HashSet<Animais>();

        }

        /// <summary>
        /// Identificador do Reserva
        /// </summary>
        [Key]
        public int IdReserva { get; set; }


        /// <summary>
        /// Data de Entrada
        /// </summary>
        [Display(Name = "Data de Entrada")]
        public DateTime D_Entrada { get; set; }


        /// <summary>
        /// Data de Saida
        /// </summary>
        [Display(Name = "Data de Saída")]
        public DateTime D_Saida { get; set; }

        /// Numero de animais
        /// </summary>
        [Required(ErrorMessage = "O número de animais é de preenchimento obrigatório")]
        [Display(Name = "Número de Animais")]
        public int Nr_animais { get; set; }


        /// Numero de registo do Animal
        /// </summary>
        [Required(ErrorMessage = "O número de registo é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "O número de registo não pode ter mais de 60 caracteres.")]
        [Display(Name = "Número de Registo")]
        public string Nr_registo { get; set; }

        //********************************************************************************
        public ICollection<Animais> ListaAnimais { get; set; }

        //********************************************************************************

        /// <summary>
        /// FK para identificar o CLiente
        /// </summary>
        [ForeignKey(nameof(Cliente))]
        public int IdCliente { get; set; }
        public Clientes Cliente { get; set; }


        /// <summary>
        /// FK para identificar a box
        /// </summary>
        /// 
        [Display(Name = "Box")]
        [ForeignKey(nameof(Box))]
        public int IdBox { get; set; } 

        public Boxs Box { get; set; }

        //********************************************************************************

    }


}
