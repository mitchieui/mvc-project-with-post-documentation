using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyMvcApp.Models
{
    public class MvcDbContext : DbContext
{
    public MvcDbContext(DbContextOptions<MvcDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students{get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentNumber);

            entity.ToTable("students");
            entity.Property(e => e.StudentNumber)
                .HasColumnName("student_number");
            entity.Property(e => e.FirstName)
                .HasColumnName("first_name");
            entity.Property(e => e.MiddleName)
                .HasColumnName("middle_name");
            entity.Property(e => e.LastName)
                .HasColumnName("last_name");
                entity.Property(e => e.ProgramId)
                .HasColumnName("program_id");
            entity.HasOne(e => e.Program)
                .WithMany(e => e.Students)
                .HasForeignKey(e => e.ProgramId);
        });

        modelBuilder.Entity<CollegeProgram>(entity =>
        {
            entity.HasKey(e => e.ProgramId);

            entity.ToTable("programs");
            entity.Property(e => e.ProgramId)
                .HasColumnName("program_id");
            entity.Property(e => e.ProgramName)
                .HasColumnName("program_name");
        });
    }
}
}
