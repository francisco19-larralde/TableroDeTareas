using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class TableroContext : DbContext
    {
        public TableroContext(DbContextOptions<TableroContext> options) : base(options)
        {
        }

        public DbSet<Tablero> Tableros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Columna> Columnas { get; set; }
        public DbSet<Tarea> Tareas { get; set; }


    }
}
