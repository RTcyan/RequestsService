using RequestsService.Domain.Model;
using RequestsService.Domain.Model.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RequestsService.Domain.DB
{
    public class ServiceDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        /// <summary>
        /// Пользователи
        /// </summary>
        public override DbSet<User> Users { get; set; }

        /// <summary>
        /// Заявки
        /// </summary>
        public DbSet<Request> Requests { get; private set; }

        /// <summary>
        /// Отделы
        /// </summary>
        public DbSet<Department> Departments { get; private set; }

        /// <summary>
        /// Факультеты
        /// </summary>
        public DbSet<Faculty> Faculties { get; private set; }

        /// <summary>
        /// Операторы
        /// </summary>
        public DbSet<Operator> Operators { get; private set; }

        /// <summary>
        /// Типы заявок
        /// </summary>
        public DbSet<RequestsType> RequestsTypes { get; private set; }

        /// <summary>
        /// Студенты
        /// </summary>
        public DbSet<Student> Students { get; private set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public DbSet<Employee> Employees { get; private set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region User

            modelBuilder.Entity<User>(x =>
            {
                x.HasOne(y => y.Employee)
                    .WithOne()
                    .HasForeignKey<User>("EmployeeId")
                    .IsRequired(true);
                x.HasIndex("EmployeeId").IsUnique(true);
            });

            #endregion

            #region Employee

            modelBuilder.Entity<Employee>(x =>
            {
                x.Property(y => y.FirstName)
                    .HasColumnName("FirstName")
                    .IsRequired();
                x.Property(y => y.Surname)
                    .HasColumnName("Surname")
                    .IsRequired();
                x.HasOne(y => y.Student)
                    .WithOne(x => x.Employee)
                    .HasForeignKey<Student>("EmployeeId");
                x.HasOne(y => y.Operator)
                    .WithOne(x => x.Employee)
                    .HasForeignKey<Operator>("EmployeeId");
                x.Ignore(y => y.FullName);
            });

            #endregion

            #region Faculty

            modelBuilder.Entity<Faculty>(x =>
            {
                x.ToTable("Faculties");
                EntityId(x);
                x.Property(y => y.Name)
                    .HasColumnName("Name")
                    .IsRequired();
            });

            #endregion

            #region Student

            modelBuilder.Entity<Student>(b =>
            {
                b.ToTable("Students");
                EntityId(b);
                b.HasOne(x => x.Faculty)
                    .WithMany()
                    .IsRequired();
                b.Property(x => x.Grade)
                    .HasColumnName("Grade")
                    .IsRequired();
                b.Property(x => x.StartEducation)
                    .HasColumnName("StartEducation")
                    .IsRequired();
                b.Property(x => x.NumberStudentCard)
                    .HasColumnName("NumberStudentCard")
                    .IsRequired();
                b.Property(x => x.PhotoStudentCardId)
                    .HasColumnName("PhotoStudentCardId");
            });

            #endregion

            #region Department

            modelBuilder.Entity<Department>(x =>
            {
                x.ToTable("Departments");
                EntityId(x);
                x.Property(y => y.Name)
                    .HasColumnName("Name")
                    .IsRequired();
            });

            #endregion

            #region Operator

            modelBuilder.Entity<Operator>(b =>
            {
                b.ToTable("Operators");
                EntityId(b);
                b.HasOne(x => x.Department)
                    .WithMany()
                    .IsRequired();
            });

            #endregion

            #region RequestType

            modelBuilder.Entity<RequestsType>(x =>
            {
                x.ToTable("RequestsTypes");
                EntityId(x);
                x.Property(y => y.Name)
                    .HasColumnName("Name")
                    .IsRequired();
                x.Property(y => y.Fields)
                    .HasColumnName("Fields")
                    .IsRequired();
                x.HasOne(x => x.Department)
                    .WithOne()
                    .HasForeignKey<RequestsType>("DepartmentId")
                    .IsRequired();
            });

            #endregion

            #region Request

            modelBuilder.Entity<Request>(x =>
            {
                x.ToTable("Requests");
                EntityId(x);
                x.HasOne(y => y.Type)
                    .WithMany()
                    .IsRequired();
                x.Property(y => y.Data)
                    .HasColumnName("Data")
                    .IsRequired();
                x.Property(y => y.ResultFileId)
                    .HasColumnName("Result");
                x.Property(y => y.Created)
                    .HasColumnName("CreationDate")
                    .IsRequired();
                x.Property(y => y.ProcessingStartDate)
                    .HasColumnName("ProcessingStartDate");
                x.Property(y => y.ProcessingEndDate)
                    .HasColumnName("ProcessingEndDate");
                x.HasOne(y => y.Operator)
                    .WithMany();
                x.Property(y => y.UserComment)
                    .HasColumnName("UserComment");
                x.Property(y => y.OperatorComment)
                    .HasColumnName("OperatorComment");
                x.Property(y => y.RequestStatus)
                    .HasColumnName("RequestStatus")
                    .IsRequired();
            });

            #endregion

        }

        /// <summary>
        /// Описание идентификатора сущности модели
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="builder">Построитель модели данных</param>
        private static void EntityId<TEntity>(EntityTypeBuilder<TEntity> builder)
                where TEntity : Entity
        {
            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired();
            builder.HasKey(x => x.Id)
                .HasAnnotation("Npgsql:Serial", true);
        }
    }

}
