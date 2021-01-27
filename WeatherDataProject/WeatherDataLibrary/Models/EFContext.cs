
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataLibrary.Models
{
    class EFContext : DbContext
    {
        private string connectionString;
        public EFContext() : base()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("jsconfig1.json", optional: false);

            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("sqlConnection").ToString();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);

        }

        public DbSet<WeatherData> MeasuringData { get; set; }

    }
}
