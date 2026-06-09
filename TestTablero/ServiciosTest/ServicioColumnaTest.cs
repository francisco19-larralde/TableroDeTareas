using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTablero.ServiciosTest
{
    public class ServicioColumnaTest
    {
        private readonly DbContextOptions<TableroContext> _context;

        public ServicioColumnaTest()
        {

            _context = new DbContextOptionsBuilder<TableroContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }
    }
}
