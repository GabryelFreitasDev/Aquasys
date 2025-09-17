using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aquasys.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSyncForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserGlobalId",
                table: "Vessels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VesselGlobalId",
                table: "VesselImages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VesselGlobalId",
                table: "Inspections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VesselGlobalId",
                table: "Holds",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HoldGlobalId",
                table: "HoldInspections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "InspectionGlobalId",
                table: "HoldInspections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HoldGlobalId",
                table: "HoldImages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HoldInspectionGlobalId",
                table: "HoldConditions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HoldGlobalId",
                table: "HoldCargos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGlobalId",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "VesselGlobalId",
                table: "VesselImages");

            migrationBuilder.DropColumn(
                name: "VesselGlobalId",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "VesselGlobalId",
                table: "Holds");

            migrationBuilder.DropColumn(
                name: "HoldGlobalId",
                table: "HoldInspections");

            migrationBuilder.DropColumn(
                name: "InspectionGlobalId",
                table: "HoldInspections");

            migrationBuilder.DropColumn(
                name: "HoldGlobalId",
                table: "HoldImages");

            migrationBuilder.DropColumn(
                name: "HoldInspectionGlobalId",
                table: "HoldConditions");

            migrationBuilder.DropColumn(
                name: "HoldGlobalId",
                table: "HoldCargos");
        }
    }
}
