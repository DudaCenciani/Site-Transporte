﻿using Microsoft.EntityFrameworkCore;
using AgendamentoTransporte.Models;

namespace AgendamentoTransporte.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Agendamento> Agendamentos { get; set; }
    }
}
