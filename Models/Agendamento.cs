using System;
using System.ComponentModel.DataAnnotations;

namespace AgendamentoTransporte.Models
{
    public class Agendamento
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Informe o nome do paciente.")]
        public string NomePaciente { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o telefone.")]
        [Phone]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        public string LocalConsulta { get; set; } = string.Empty;
        public string? LocalConsultaOutro { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }  // Não precisa inicializar

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }  // Também não precisa

        [Required]
        public string PrecisaAcompanhante { get; set; } = string.Empty;

        [Required]
        public string LocalBusca { get; set; }
        public string? LocalBuscaOutro { get; set; } = string.Empty;

        public string? PontoReferencia { get; set; } = string.Empty;

        public string? MotivoViagem { get; set; } = string.Empty;

        public string? Observacao { get; set; }

        public string? UsuarioResponsavel { get; set; } = string.Empty;
    }
}
