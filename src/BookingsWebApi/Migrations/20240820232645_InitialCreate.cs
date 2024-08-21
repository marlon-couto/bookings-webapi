using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingsWebApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    State = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Salt = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Role = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hotels_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Image = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuestQuantity = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "State", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1e1ca2ff-eadb-45fd-a9c0-49384900f40b"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5439), false, "City 2", "State 2", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5435) },
                    { new Guid("ca4c3b99-c156-40b9-b3e2-9616d5cf05d2"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5434), false, "City 1", "State 1", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5431) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsDeleted", "Name", "Password", "Role", "Salt", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("05a347ae-1367-4e3b-b805-def7e8d901a6"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5301), "user1@mail.com", false, "User 1", "Of5n7kc14AwibmiFXk5ZnyDbioxlCiqxcZr7ayc/ad4=", "Client", "RDsG9pM5F0tj6ZIU2UogSA==", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5261) },
                    { new Guid("498435d1-be9c-4f7f-8832-b3b043915c76"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5306), "user2@mail.com", false, "User 2", "7eyPpIQlkJGpp/lfFJuSeJ3IZbo5DznSu/C1VG7JO9I=", "Client", "Gqude0JZ38YVvz/RUTeZ9w==", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5302) },
                    { new Guid("6eff8e4c-a440-44ef-a001-6a4f4fa5f102"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5311), "user3@mail.com", false, "User 3", "sT+OPanSmual937atM35GfT2Xp1w5yPXpTBA4U8JdEo=", "Admin", "pWIyxOGXD5363rNbX0ASfg==", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5308) }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CityId", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("8c2ef74e-6c01-432d-b349-7867d112ba62"), "Address 2", new Guid("1e1ca2ff-eadb-45fd-a9c0-49384900f40b"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5457), false, "Hotel 2", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5454) },
                    { new Guid("b86aeb7a-a1a0-49ff-b9cb-5ff6ec7f7302"), "Address 1", new Guid("ca4c3b99-c156-40b9-b3e2-9616d5cf05d2"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5453), false, "Hotel 1", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5449) }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "CreatedAt", "HotelId", "Image", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("5a568cf4-7b13-4e22-8779-1fffb3e13d50"), 2, new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5484), new Guid("8c2ef74e-6c01-432d-b349-7867d112ba62"), "Image 4", false, "Room 4", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5482) },
                    { new Guid("7da42056-e3bd-4aae-8831-db637bb0a41e"), 3, new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5480), new Guid("b86aeb7a-a1a0-49ff-b9cb-5ff6ec7f7302"), "Image 3", false, "Room 3", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5478) },
                    { new Guid("7fd47a35-252b-4c1d-85cd-ba11629ade88"), 1, new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5476), new Guid("b86aeb7a-a1a0-49ff-b9cb-5ff6ec7f7302"), "Image 2", false, "Room 2", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5473) },
                    { new Guid("9aba51ec-a386-4697-8d0f-9487ae7d5516"), 3, new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5492), new Guid("8c2ef74e-6c01-432d-b349-7867d112ba62"), "Image 6", false, "Room 6", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5489) },
                    { new Guid("cdd0c01b-d226-4c98-93ac-1aa199b55d3c"), 2, new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5471), new Guid("b86aeb7a-a1a0-49ff-b9cb-5ff6ec7f7302"), "Image 1", false, "Room 1", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5467) },
                    { new Guid("d7df68b1-b57e-4626-8f91-9c9b9afd16f8"), 1, new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5488), new Guid("8c2ef74e-6c01-432d-b349-7867d112ba62"), "Image 5", false, "Room 5", new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5485) }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CheckIn", "CheckOut", "CreatedAt", "GuestQuantity", "IsDeleted", "RoomId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("0300ec2e-bb50-4542-8aa2-167ea2721dc6"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5524), new DateTime(2024, 8, 21, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5525), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5527), 1, false, new Guid("7fd47a35-252b-4c1d-85cd-ba11629ade88"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5522), new Guid("498435d1-be9c-4f7f-8832-b3b043915c76") },
                    { new Guid("5c75464d-b6cc-4f5c-927b-e324935dc0c1"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5506), new DateTime(2024, 8, 21, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5507), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5514), 1, false, new Guid("cdd0c01b-d226-4c98-93ac-1aa199b55d3c"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5504), new Guid("05a347ae-1367-4e3b-b805-def7e8d901a6") },
                    { new Guid("8671b4af-0a11-45a6-8a23-a32f5890bbf9"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5517), new DateTime(2024, 8, 21, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5518), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5521), 1, false, new Guid("5a568cf4-7b13-4e22-8779-1fffb3e13d50"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5516), new Guid("05a347ae-1367-4e3b-b805-def7e8d901a6") },
                    { new Guid("ed87365b-ff61-48f3-a7d5-b263a352c381"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5531), new DateTime(2024, 8, 21, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5532), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5534), 1, false, new Guid("d7df68b1-b57e-4626-8f91-9c9b9afd16f8"), new DateTime(2024, 8, 20, 23, 26, 45, 667, DateTimeKind.Utc).AddTicks(5529), new Guid("498435d1-be9c-4f7f-8832-b3b043915c76") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_CityId",
                table: "Hotels",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms",
                column: "HotelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
