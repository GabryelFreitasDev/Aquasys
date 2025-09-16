using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Aquasys.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeVessels",
                columns: table => new
                {
                    idtypevessel = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeVessels", x => x.idtypevessel);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    iduser = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    rememberme = table.Column<bool>(type: "boolean", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.iduser);
                });

            migrationBuilder.CreateTable(
                name: "Vessels",
                columns: table => new
                {
                    idvessel = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vesselname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    place = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    imo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    portregistry = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    manufacturingdate = table.Column<DateTime>(type: "date", nullable: true),
                    flag = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    vesseltype = table.Column<int>(type: "integer", nullable: false),
                    owner = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "character varying(200)", maxLength: 200, nullable: false),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    iduserregistration = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vessels", x => x.idvessel);
                    table.ForeignKey(
                        name: "FK_Vessels_Users_iduserregistration",
                        column: x => x.iduserregistration,
                        principalTable: "Users",
                        principalColumn: "iduser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Holds",
                columns: table => new
                {
                    idhold = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    capacity = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    loadplan = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    productweight = table.Column<decimal>(type: "numeric(18,5)", nullable: false),
                    lastcargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    secondlastcargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    thirdlastcargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fourthlastcargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    idvessel = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holds", x => x.idhold);
                    table.ForeignKey(
                        name: "FK_Holds_Vessels_idvessel",
                        column: x => x.idvessel,
                        principalTable: "Vessels",
                        principalColumn: "idvessel",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    idinspection = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    os = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    startdatetime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    enddatetime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    shippingagent = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    leadinspector = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    idvessel = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.idinspection);
                    table.ForeignKey(
                        name: "FK_Inspections_Vessels_idvessel",
                        column: x => x.idvessel,
                        principalTable: "Vessels",
                        principalColumn: "idvessel",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VesselImages",
                columns: table => new
                {
                    idvesselimage = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image = table.Column<byte[]>(type: "bytea", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    idvessel = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VesselImages", x => x.idvesselimage);
                    table.ForeignKey(
                        name: "FK_VesselImages_Vessels_idvessel",
                        column: x => x.idvessel,
                        principalTable: "Vessels",
                        principalColumn: "idvessel",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoldCargos",
                columns: table => new
                {
                    idholdcargo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cargo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    order = table.Column<int>(type: "integer", nullable: false),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    idhold = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoldCargos", x => x.idholdcargo);
                    table.ForeignKey(
                        name: "FK_HoldCargos_Holds_idhold",
                        column: x => x.idhold,
                        principalTable: "Holds",
                        principalColumn: "idhold",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoldImages",
                columns: table => new
                {
                    idholdimage = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image = table.Column<byte[]>(type: "bytea", nullable: false),
                    description = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    observation = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    idhold = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoldImages", x => x.idholdimage);
                    table.ForeignKey(
                        name: "FK_HoldImages_Holds_idhold",
                        column: x => x.idhold,
                        principalTable: "Holds",
                        principalColumn: "idhold",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoldInspections",
                columns: table => new
                {
                    idholdinspection = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    idinspection = table.Column<long>(type: "bigint", nullable: false),
                    idhold = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoldInspections", x => x.idholdinspection);
                    table.ForeignKey(
                        name: "FK_HoldInspections_Holds_idhold",
                        column: x => x.idhold,
                        principalTable: "Holds",
                        principalColumn: "idhold",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoldInspections_Inspections_idinspection",
                        column: x => x.idinspection,
                        principalTable: "Inspections",
                        principalColumn: "idinspection",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoldConditions",
                columns: table => new
                {
                    idholdcondition = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    empty = table.Column<int>(type: "integer", nullable: false),
                    clean = table.Column<int>(type: "integer", nullable: false),
                    dry = table.Column<int>(type: "integer", nullable: false),
                    odorfree = table.Column<int>(type: "integer", nullable: false),
                    cargoresidue = table.Column<int>(type: "integer", nullable: false),
                    insects = table.Column<int>(type: "integer", nullable: false),
                    cleaningmethod = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    idholdinspection = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoldConditions", x => x.idholdcondition);
                    table.ForeignKey(
                        name: "FK_HoldConditions_HoldInspections_idholdinspection",
                        column: x => x.idholdinspection,
                        principalTable: "HoldInspections",
                        principalColumn: "idholdinspection",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoldCargos_GlobalId",
                table: "HoldCargos",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoldCargos_idhold",
                table: "HoldCargos",
                column: "idhold");

            migrationBuilder.CreateIndex(
                name: "IX_HoldConditions_GlobalId",
                table: "HoldConditions",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoldConditions_idholdinspection",
                table: "HoldConditions",
                column: "idholdinspection");

            migrationBuilder.CreateIndex(
                name: "IX_HoldImages_GlobalId",
                table: "HoldImages",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoldImages_idhold",
                table: "HoldImages",
                column: "idhold");

            migrationBuilder.CreateIndex(
                name: "IX_HoldInspections_GlobalId",
                table: "HoldInspections",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoldInspections_idhold",
                table: "HoldInspections",
                column: "idhold");

            migrationBuilder.CreateIndex(
                name: "IX_HoldInspections_idinspection",
                table: "HoldInspections",
                column: "idinspection");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_GlobalId",
                table: "Holds",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Holds_idvessel",
                table: "Holds",
                column: "idvessel");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_GlobalId",
                table: "Inspections",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_idvessel",
                table: "Inspections",
                column: "idvessel");

            migrationBuilder.CreateIndex(
                name: "IX_TypeVessels_GlobalId",
                table: "TypeVessels",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GlobalId",
                table: "Users",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VesselImages_GlobalId",
                table: "VesselImages",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VesselImages_idvessel",
                table: "VesselImages",
                column: "idvessel");

            migrationBuilder.CreateIndex(
                name: "IX_Vessels_GlobalId",
                table: "Vessels",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vessels_iduserregistration",
                table: "Vessels",
                column: "iduserregistration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoldCargos");

            migrationBuilder.DropTable(
                name: "HoldConditions");

            migrationBuilder.DropTable(
                name: "HoldImages");

            migrationBuilder.DropTable(
                name: "TypeVessels");

            migrationBuilder.DropTable(
                name: "VesselImages");

            migrationBuilder.DropTable(
                name: "HoldInspections");

            migrationBuilder.DropTable(
                name: "Holds");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "Vessels");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
