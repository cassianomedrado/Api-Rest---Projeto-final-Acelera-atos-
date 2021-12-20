﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using netbullAPI.Persistencia;

#nullable disable

namespace netbullAPI.Migrations
{
    [DbContext(typeof(netbullDBContext))]
    [Migration("20211201011045_AlterTableforekeyTelefone")]
    partial class AlterTableforekeyTelefone
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("netbullAPI.Entidade.Endereco", b =>
                {
                    b.Property<int>("endereco_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("endereco_id"));

                    b.Property<string>("endereco_complemento")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("endereco_idpessoa")
                        .HasColumnType("integer");

                    b.Property<string>("endereco_logradouro")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("endereco_numero")
                        .HasColumnType("integer");

                    b.HasKey("endereco_id");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("netbullAPI.Entidade.Pessoa", b =>
                {
                    b.Property<int>("pessoa_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("pessoa_id"));

                    b.Property<int>("pessoa_documento")
                        .HasColumnType("integer");

                    b.Property<string>("pessoa_nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("pessoa_tipopessoa")
                        .HasColumnType("integer");

                    b.HasKey("pessoa_id");

                    b.ToTable("Pessoas");
                });

            modelBuilder.Entity("netbullAPI.Entidade.Telefone", b =>
                {
                    b.Property<int>("telefone_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("telefone_id"));

                    b.Property<int>("telefone_idPessoa")
                        .HasColumnType("integer");

                    b.Property<int>("telefone_numero")
                        .HasColumnType("integer");

                    b.HasKey("telefone_id");

                    b.HasIndex("telefone_idPessoa");

                    b.ToTable("Telefones");
                });

            modelBuilder.Entity("netbullAPI.Entidade.Telefone", b =>
                {
                    b.HasOne("netbullAPI.Entidade.Pessoa", "Pessoa")
                        .WithMany()
                        .HasForeignKey("telefone_idPessoa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pessoa");
                });
#pragma warning restore 612, 618
        }
    }
}
