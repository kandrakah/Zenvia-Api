using SampleApp.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using Zenvia.Api.Models.Requests;
using Zenvia.Api.Models.Responses;

namespace SampleApp.Context
{
    public class AppDBContext : DbContext
    {
        private static string BasePath = $"{AppDomain.CurrentDomain.BaseDirectory}Data\\";

        public DbSet<MessageModel> Messages { get; set; }
        
        public AppDBContext() : base($"Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=SampleDB;Integrated Security=SSPI;AttachDBFilename={BasePath}SampleDB.mdf") { }

        public void CreateDB()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            this.Database.CreateIfNotExists();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<MessageModel>().ToTable("Messages");
            modelBuilder.Entity<MessageModel>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
