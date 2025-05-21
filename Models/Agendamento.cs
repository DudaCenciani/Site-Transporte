using System;
using System.ComponentModel.DataAnnotations;

namespace AgendamentoTransporte.Models
{
    public class Agendamento
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Informe o nome do paciente.")]
        public string NomePaciente { get; set; }

        [Required(ErrorMessage = "Informe o telefone.")]
        [Phone]
        public string Telefone { get; set; }

        [Required]
        public string LocalConsulta { get; set; }
        public string? LocalConsultaOutro { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        [Required]
        public string PrecisaAcompanhante { get; set; }

        [Required]
        public string LocalBusca { get; set; }
        public string? LocalBuscaOutro { get; set; }

        public string? PontoReferencia { get; set; }

        public string? MotivoViagem { get; set; }

        public string? Observacao { get; set; }

        public string? UsuarioResponsavel { get; set; }
    }
}
