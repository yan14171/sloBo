// <auto-generated />
using Alexa_proj.Data_Control;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alexa_proj.Migrations
{
    [DbContext(typeof(FunctionalContext))]
    [Migration("20210517193206_FunctionalModel_01")]
    partial class FunctionalModel_01
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.ExecutableModel", b =>
                {
                    b.Property<int>("ExecutableModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("executable_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExecutableName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("executable_name");

                    b.HasKey("ExecutableModelId");

                    b.ToTable("Executables");
                });

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.Function", b =>
                {
                    b.Property<int>("FunctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("function_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExecutableId")
                        .HasColumnType("int");

                    b.Property<string>("FunctionEndpoint")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("function_endpoint");

                    b.Property<string>("FunctionName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("function_name");

                    b.HasKey("FunctionId");

                    b.HasIndex("ExecutableId")
                        .IsUnique();

                    b.ToTable("Functions");
                });

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.FunctionResult", b =>
                {
                    b.Property<int>("FunctionResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("result_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FunctionId")
                        .HasColumnType("int");

                    b.Property<string>("ResultValue")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("result_value");

                    b.HasKey("FunctionResultId");

                    b.HasIndex("FunctionId")
                        .IsUnique();

                    b.ToTable("FunctionResults");
                });

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.Keyword", b =>
                {
                    b.Property<int>("KeywordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("keyword_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExecutableId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCritical")
                        .HasColumnType("bit");

                    b.Property<string>("KeywordValue")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("keyword_value");

                    b.HasKey("KeywordId");

                    b.HasIndex("ExecutableId");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.Function", b =>
                {
                    b.HasOne("Alexa_proj.Data_Control.Models.ExecutableModel", "Executable")
                        .WithOne("ExecutableFunction")
                        .HasForeignKey("Alexa_proj.Data_Control.Models.Function", "ExecutableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Executable");
                });

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.FunctionResult", b =>
                {
                    b.HasOne("Alexa_proj.Data_Control.Models.Function", "Function")
                        .WithOne("FunctionResult")
                        .HasForeignKey("Alexa_proj.Data_Control.Models.FunctionResult", "FunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Function");
                });

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.Keyword", b =>
                {
                    b.HasOne("Alexa_proj.Data_Control.Models.ExecutableModel", "Executable")
                        .WithMany("Keywords")
                        .HasForeignKey("ExecutableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Executable");
                });

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.ExecutableModel", b =>
                {
                    b.Navigation("ExecutableFunction")
                        .IsRequired();

                    b.Navigation("Keywords");
                });

            modelBuilder.Entity("Alexa_proj.Data_Control.Models.Function", b =>
                {
                    b.Navigation("FunctionResult");
                });
#pragma warning restore 612, 618
        }
    }
}
