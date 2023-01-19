﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Models.AppDbContext;

#nullable disable

namespace Bank.Data.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    [Migration("20230118103821_Added_AuditFields_For_BankTable")]
    partial class AddedAuditFieldsForBankTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bank.Models.BankDetail", b =>
                {
                    b.Property<int>("BankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BankId"));

                    b.Property<string>("BankCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsAtive")
                        .HasColumnType("int");

                    b.HasKey("BankId");

                    b.ToTable("BankDetails");
                });

            modelBuilder.Entity("Bank.Models.Currency", b =>
                {
                    b.Property<string>("CurrencyCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ConversionValue")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CurrencyCode");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("Bank.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("EmployeeEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Bank.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreditedAccount")
                        .HasColumnType("int");

                    b.Property<int>("DepositedAccount")
                        .HasColumnType("int");

                    b.Property<int>("ReceiverUserId")
                        .HasColumnType("int");

                    b.Property<int>("SenderUserId")
                        .HasColumnType("int");

                    b.Property<double>("ServiceCharge")
                        .HasColumnType("float");

                    b.Property<string>("TransactionCurrency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionUniqueId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Bank.Models.User", b =>
                {
                    b.Property<int>("User_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("User_Id"));

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("BankCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniqueUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPhone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("User_Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
