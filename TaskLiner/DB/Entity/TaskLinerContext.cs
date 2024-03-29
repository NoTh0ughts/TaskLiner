﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TaskLiner.Service;

#nullable disable

namespace TaskLiner.DB.Entity
{
    public partial class TaskLinerContext : DbContext
    {
        public TaskLinerContext()
        {
        }

        public TaskLinerContext(DbContextOptions<TaskLinerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectAccess> ProjectAccesses { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskComment> TaskComments { get; set; }
        public virtual DbSet<TaskSubscriber> TaskSubscribers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }
        public virtual DbSet<WorkerContract> WorkerContracts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(ConfigLoader.MySqlURL);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("company");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project");

                entity.HasIndex(e => e.CompanyId, "company_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("company_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Scope)
                    .HasMaxLength(500)
                    .HasColumnName("scope");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("project_ibfk_1");
            });

            modelBuilder.Entity<ProjectAccess>(entity =>
            {
                entity.ToTable("project_access");

                entity.HasIndex(e => e.ProjectId, "project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ContractId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("contract_id");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("project_id");

                entity.HasOne(d => d.Contract)
                    .WithOne(p => p.ProjectAccess)
                    .HasForeignKey<ProjectAccess>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("project_access_ibfk_1");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectAccesses)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("project_access_ibfk_2");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("task");

                entity.HasIndex(e => e.ProjectId, "project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.AddDate).HasColumnName("add_date");

                entity.Property(e => e.Checklist)
                    .HasMaxLength(1000)
                    .HasColumnName("checklist");

                entity.Property(e => e.ColumnState)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("column_state");

                entity.Property(e => e.Content)
                    .HasMaxLength(500)
                    .HasColumnName("content");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("project_id");

                entity.Property(e => e.SpendedTime).HasColumnName("spended_time");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("state");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_ibfk_1");
            });

            modelBuilder.Entity<TaskComment>(entity =>
            {
                entity.ToTable("task_comment");

                entity.HasIndex(e => e.TaskId, "task_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.SpendedTime).HasColumnName("spended_time");

                entity.Property(e => e.TaskId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("task_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskComments)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_comment_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaskComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_comment_ibfk_1");
            });

            modelBuilder.Entity<TaskSubscriber>(entity =>
            {
                entity.ToTable("task_subscribers");

                entity.HasIndex(e => e.TaskId, "task_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.TaskId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("task_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskSubscribers)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_subscribers_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaskSubscribers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_subscribers_ibfk_2");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(50)
                    .HasColumnName("avatar");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("email");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fullname");

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("nickname");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Proffesion)
                    .HasMaxLength(100)
                    .HasColumnName("proffesion");
            });

            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.ToTable("user_tasks");

                entity.HasIndex(e => e.TaskId, "task_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.HoursWorked).HasColumnName("hours_worked");

                entity.Property(e => e.IsOwner).HasColumnName("is_owner");

                entity.Property(e => e.TaskId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("task_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.UserTasks)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_tasks_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_tasks_ibfk_2");
            });

            modelBuilder.Entity<WorkerContract>(entity =>
            {
                entity.ToTable("worker_contract");

                entity.HasIndex(e => e.CompanyId, "company_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("company_id");

                entity.Property(e => e.IsOwner).HasColumnName("is_owner");

                entity.Property(e => e.UserId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.WorkerContracts)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("worker_contract_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkerContracts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("worker_contract_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
