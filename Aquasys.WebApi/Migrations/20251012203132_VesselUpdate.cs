using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aquasys.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class VesselUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "manufacturingdate",
                table: "Vessels",
                newName: "dateofbuilding");

            migrationBuilder.AddColumn<string>(
                name: "dockinglocation",
                table: "Vessels",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "fourthlastcargo",
                table: "Vessels",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastcargo",
                table: "Vessels",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "os",
                table: "Vessels",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "secondlastcargo",
                table: "Vessels",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shippingagent",
                table: "Vessels",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "thirdlastcargo",
                table: "Vessels",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dockinglocation",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "fourthlastcargo",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "lastcargo",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "os",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "secondlastcargo",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "shippingagent",
                table: "Vessels");

            migrationBuilder.DropColumn(
                name: "thirdlastcargo",
                table: "Vessels");

            migrationBuilder.RenameColumn(
                name: "dateofbuilding",
                table: "Vessels",
                newName: "manufacturingdate");

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
    }
}
