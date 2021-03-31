﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreAspShop.Migrations
{
    public partial class AppointmentsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDay = table.Column<DateTime>(nullable: false),
                    CustomerName = table.Column<string>(maxLength: 30, nullable: false),
                    CustomerPhoneNumber = table.Column<string>(maxLength: 25, nullable: false),
                    CustomerEmail = table.Column<string>(nullable: false),
                    IsConfirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsForAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsForAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsForAppointments_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsForAppointments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsForAppointments_AppointmentId",
                table: "ProductsForAppointments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsForAppointments_ProductId",
                table: "ProductsForAppointments",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsForAppointments");

            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
