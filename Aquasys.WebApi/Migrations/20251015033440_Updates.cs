using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Aquasys.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoldInspections_Holds_idhold",
                table: "HoldInspections");

            migrationBuilder.DropForeignKey(
                name: "FK_HoldInspections_Inspections_idinspection",
                table: "HoldInspections");

            migrationBuilder.DropForeignKey(
                name: "FK_Holds_Vessels_idvessel",
                table: "Holds");

            migrationBuilder.DropForeignKey(
                name: "FK_VesselImages_Vessels_idvessel",
                table: "VesselImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Vessels_Users_iduserregistration",
                table: "Vessels");

            migrationBuilder.DropTable(
                name: "HoldCargos");

            migrationBuilder.DropTable(
                name: "HoldConditions");

            migrationBuilder.DropTable(
                name: "HoldImages");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "TypeVessels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vessels",
                table: "Vessels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VesselImages",
                table: "VesselImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Holds",
                table: "Holds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoldInspections",
                table: "HoldInspections");

            migrationBuilder.DropIndex(
                name: "IX_HoldInspections_idinspection",
                table: "HoldInspections");

            migrationBuilder.DropColumn(
                name: "operator",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "vesseltype",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "fourthlastcargo",
                table: "Holds");

            migrationBuilder.DropColumn(
                name: "lastcargo",
                table: "Holds");

            migrationBuilder.DropColumn(
                name: "secondlastcargo",
                table: "Holds");

            migrationBuilder.DropColumn(
                name: "thirdlastcargo",
                table: "Holds");

            migrationBuilder.DropColumn(
                name: "idinspection",
                table: "HoldInspections");

            migrationBuilder.RenameTable(
                name: "Vessels",
                newName: "vessel");

            migrationBuilder.RenameTable(
                name: "VesselImages",
                newName: "vesselimage");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Holds",
                newName: "hold");

            migrationBuilder.RenameTable(
                name: "HoldInspections",
                newName: "holdinspection");

            migrationBuilder.RenameColumn(
                name: "place",
                table: "vessel",
                newName: "vesseloperator");

            migrationBuilder.RenameIndex(
                name: "IX_Vessels_iduserregistration",
                table: "vessel",
                newName: "IX_vessel_iduserregistration");

            migrationBuilder.RenameIndex(
                name: "IX_Vessels_GlobalId",
                table: "vessel",
                newName: "IX_vessel_GlobalId");

            migrationBuilder.RenameIndex(
                name: "IX_VesselImages_idvessel",
                table: "vesselimage",
                newName: "IX_vesselimage_idvessel");

            migrationBuilder.RenameIndex(
                name: "IX_VesselImages_GlobalId",
                table: "vesselimage",
                newName: "IX_vesselimage_GlobalId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GlobalId",
                table: "user",
                newName: "IX_user_GlobalId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "hold",
                newName: "basementnumber");

            migrationBuilder.RenameIndex(
                name: "IX_Holds_idvessel",
                table: "hold",
                newName: "IX_hold_idvessel");

            migrationBuilder.RenameIndex(
                name: "IX_Holds_GlobalId",
                table: "hold",
                newName: "IX_hold_GlobalId");

            migrationBuilder.RenameColumn(
                name: "datacadastro",
                table: "holdinspection",
                newName: "registrationdatetime");

            migrationBuilder.RenameIndex(
                name: "IX_HoldInspections_idhold",
                table: "holdinspection",
                newName: "IX_holdinspection_idhold");

            migrationBuilder.RenameIndex(
                name: "IX_HoldInspections_GlobalId",
                table: "holdinspection",
                newName: "IX_holdinspection_GlobalId");

            migrationBuilder.AddColumn<string>(
                name: "agent",
                table: "hold",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "cargoresidue",
                table: "holdinspection",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "clean",
                table: "holdinspection",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "cleaningmethod",
                table: "holdinspection",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dry",
                table: "holdinspection",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "empty",
                table: "holdinspection",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "insects",
                table: "holdinspection",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "inspectiondatetime",
                table: "holdinspection",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "leadinspector",
                table: "holdinspection",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "odorfree",
                table: "holdinspection",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_vessel",
                table: "vessel",
                column: "idvessel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vesselimage",
                table: "vesselimage",
                column: "idvesselimage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "iduser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hold",
                table: "hold",
                column: "idhold");

            migrationBuilder.AddPrimaryKey(
                name: "PK_holdinspection",
                table: "holdinspection",
                column: "idholdinspection");

            migrationBuilder.CreateTable(
                name: "holdinspectionimage",
                columns: table => new
                {
                    idholdinspectionimage = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image = table.Column<byte[]>(type: "bytea", nullable: false),
                    description = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    observation = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    idholdinspection = table.Column<long>(type: "bigint", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holdinspectionimage", x => x.idholdinspectionimage);
                    table.ForeignKey(
                        name: "FK_holdinspectionimage_holdinspection_idholdinspection",
                        column: x => x.idholdinspection,
                        principalTable: "holdinspection",
                        principalColumn: "idholdinspection",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_holdinspectionimage_GlobalId",
                table: "holdinspectionimage",
                column: "GlobalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_holdinspectionimage_idholdinspection",
                table: "holdinspectionimage",
                column: "idholdinspection");

            migrationBuilder.AddForeignKey(
                name: "FK_hold_vessel_idvessel",
                table: "hold",
                column: "idvessel",
                principalTable: "vessel",
                principalColumn: "idvessel",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_holdinspection_hold_idhold",
                table: "holdinspection",
                column: "idhold",
                principalTable: "hold",
                principalColumn: "idhold",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vessel_user_iduserregistration",
                table: "vessel",
                column: "iduserregistration",
                principalTable: "user",
                principalColumn: "iduser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vesselimage_vessel_idvessel",
                table: "vesselimage",
                column: "idvessel",
                principalTable: "vessel",
                principalColumn: "idvessel",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hold_vessel_idvessel",
                table: "hold");

            migrationBuilder.DropForeignKey(
                name: "FK_holdinspection_hold_idhold",
                table: "holdinspection");

            migrationBuilder.DropForeignKey(
                name: "FK_vessel_user_iduserregistration",
                table: "vessel");

            migrationBuilder.DropForeignKey(
                name: "FK_vesselimage_vessel_idvessel",
                table: "vesselimage");

            migrationBuilder.DropTable(
                name: "holdinspectionimage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vesselimage",
                table: "vesselimage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vessel",
                table: "vessel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_holdinspection",
                table: "holdinspection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hold",
                table: "hold");

            migrationBuilder.DropColumn(
                name: "cargoresidue",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "clean",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "cleaningmethod",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "dry",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "empty",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "insects",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "inspectiondatetime",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "leadinspector",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "odorfree",
                table: "holdinspection");

            migrationBuilder.DropColumn(
                name: "agent",
                table: "hold");

            migrationBuilder.RenameTable(
                name: "vesselimage",
                newName: "VesselImages");

            migrationBuilder.RenameTable(
                name: "vessel",
                newName: "Vessels");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "holdinspection",
                newName: "HoldInspections");

            migrationBuilder.RenameTable(
                name: "hold",
                newName: "Holds");

            migrationBuilder.RenameIndex(
                name: "IX_vesselimage_idvessel",
                table: "VesselImages",
                newName: "IX_VesselImages_idvessel");

            migrationBuilder.RenameIndex(
                name: "IX_vesselimage_GlobalId",
                table: "VesselImages",
                newName: "IX_VesselImages_GlobalId");

            migrationBuilder.RenameColumn(
                name: "vesseloperator",
                table: "Vessels",
                newName: "place");

            migrationBuilder.RenameIndex(
                name: "IX_vessel_iduserregistration",
                table: "Vessels",
                newName: "IX_Vessels_iduserregistration");

            migrationBuilder.RenameIndex(
                name: "IX_vessel_GlobalId",
                table: "Vessels",
                newName: "IX_Vessels_GlobalId");

            migrationBuilder.RenameIndex(
                name: "IX_user_GlobalId",
                table: "Users",
                newName: "IX_Users_GlobalId");

            migrationBuilder.RenameColumn(
                name: "registrationdatetime",
                table: "HoldInspections",
                newName: "datacadastro");

            migrationBuilder.RenameIndex(
                name: "IX_holdinspection_idhold",
                table: "HoldInspections",
                newName: "IX_HoldInspections_idhold");

            migrationBuilder.RenameIndex(
                name: "IX_holdinspection_GlobalId",
                table: "HoldInspections",
                newName: "IX_HoldInspections_GlobalId");

            migrationBuilder.RenameColumn(
                name: "basementnumber",
                table: "Holds",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "IX_hold_idvessel",
                table: "Holds",
                newName: "IX_Holds_idvessel");

            migrationBuilder.RenameIndex(
                name: "IX_hold_GlobalId",
                table: "Holds",
                newName: "IX_Holds_GlobalId");

            migrationBuilder.AddColumn<string>(
                name: "operator",
                table: "Vessels",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "vesseltype",
                table: "Vessels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "idinspection",
                table: "HoldInspections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "fourthlastcargo",
                table: "Holds",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastcargo",
                table: "Holds",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "secondlastcargo",
                table: "Holds",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "thirdlastcargo",
                table: "Holds",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VesselImages",
                table: "VesselImages",
                column: "idvesselimage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vessels",
                table: "Vessels",
                column: "idvessel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "iduser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoldInspections",
                table: "HoldInspections",
                column: "idholdinspection");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Holds",
                table: "Holds",
                column: "idhold");

            migrationBuilder.CreateTable(
                name: "HoldCargos",
                columns: table => new
                {
                    idholdcargo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idhold = table.Column<long>(type: "bigint", nullable: false),
                    cargo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false)
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
                name: "HoldConditions",
                columns: table => new
                {
                    idholdcondition = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idholdinspection = table.Column<long>(type: "bigint", nullable: false),
                    cargoresidue = table.Column<int>(type: "integer", nullable: false),
                    clean = table.Column<int>(type: "integer", nullable: false),
                    cleaningmethod = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    dry = table.Column<int>(type: "integer", nullable: false),
                    empty = table.Column<int>(type: "integer", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    insects = table.Column<int>(type: "integer", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    odorfree = table.Column<int>(type: "integer", nullable: false),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "HoldImages",
                columns: table => new
                {
                    idholdimage = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idhold = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    image = table.Column<byte[]>(type: "bytea", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    observation = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false)
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
                name: "Inspections",
                columns: table => new
                {
                    idinspection = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idvessel = table.Column<long>(type: "bigint", nullable: false),
                    enddatetime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    leadinspector = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    os = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false),
                    shippingagent = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    startdatetime = table.Column<DateTime>(type: "timestamp", nullable: false)
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
                name: "TypeVessels",
                columns: table => new
                {
                    idtypevessel = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GlobalId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSynced = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    datacadastro = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeVessels", x => x.idtypevessel);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoldInspections_idinspection",
                table: "HoldInspections",
                column: "idinspection");

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

            migrationBuilder.AddForeignKey(
                name: "FK_HoldInspections_Holds_idhold",
                table: "HoldInspections",
                column: "idhold",
                principalTable: "Holds",
                principalColumn: "idhold",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoldInspections_Inspections_idinspection",
                table: "HoldInspections",
                column: "idinspection",
                principalTable: "Inspections",
                principalColumn: "idinspection",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Holds_Vessels_idvessel",
                table: "Holds",
                column: "idvessel",
                principalTable: "Vessels",
                principalColumn: "idvessel",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VesselImages_Vessels_idvessel",
                table: "VesselImages",
                column: "idvessel",
                principalTable: "Vessels",
                principalColumn: "idvessel",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vessels_Users_iduserregistration",
                table: "Vessels",
                column: "iduserregistration",
                principalTable: "Users",
                principalColumn: "iduser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
