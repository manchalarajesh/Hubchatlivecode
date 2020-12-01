using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TelebuHubChat.Migrations
{
    public partial class telebuhubchat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConversationId = table.Column<int>(nullable: false),
                    IsBotEnd = table.Column<byte>(nullable: false),
                    CreatedTimeUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationMessageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MessageType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationMessageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerIpInformation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HttpURL = table.Column<string>(nullable: true),
                    ClientIp = table.Column<string>(nullable: true),
                    CustomerId = table.Column<long>(nullable: false),
                    CreatedTimeUTC = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HttpReferrer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerIpInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    AccountCustomerId = table.Column<int>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WidgetAuthkeys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WidgetId = table.Column<int>(nullable: false),
                    WidgetChannelId = table.Column<int>(nullable: false),
                    AuthKey = table.Column<string>(nullable: true),
                    AuthToken = table.Column<string>(nullable: true),
                    IsActive = table.Column<byte>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetAuthkeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WidgetChannels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WidgetId = table.Column<int>(nullable: false),
                    ChanneUUID = table.Column<string>(nullable: true),
                    ConversationTypeId = table.Column<int>(nullable: false),
                    IsActive = table.Column<byte>(nullable: false),
                    RasaBotUrl = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WidgetConversationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetConversationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WidgetThemes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetThemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkFlow = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Widgets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    WidgetName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ExpiryDateUTC = table.Column<DateTime>(nullable: false),
                    PopInAfterSeconds = table.Column<sbyte>(nullable: false),
                    ThemeId = table.Column<int>(nullable: false),
                    WorkFlowId = table.Column<int>(nullable: false),
                    Purpose = table.Column<string>(nullable: true),
                    MetaData = table.Column<string>(maxLength: 1500, nullable: true),
                    MinimizeStateText = table.Column<string>(nullable: true),
                    UUID = table.Column<string>(nullable: true),
                    DomainToLoadIn = table.Column<string>(nullable: true),
                    CreatedTimeUTC = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UpdatedTimeUTC = table.Column<DateTime>(nullable: false),
                    CustomMetaData = table.Column<string>(maxLength: 1500, nullable: true),
                    AgentAndCustomerWaitTimeRestrictionInSec = table.Column<int>(nullable: false),
                    TimeToDisplayWelcomeFormToCustomer = table.Column<string>(nullable: true),
                    AutoCloseTimeForChatInMin = table.Column<int>(nullable: false),
                    WhileConnectingToAnAgent = table.Column<string>(maxLength: 1500, nullable: true),
                    CustomerWaitTimeForAgentConnect = table.Column<string>(maxLength: 1500, nullable: true),
                    BotChatClosure = table.Column<string>(maxLength: 1500, nullable: true),
                    AgentChatClosure = table.Column<string>(maxLength: 1500, nullable: true),
                    NonBusinessConnect = table.Column<string>(maxLength: 1500, nullable: true),
                    TimeSlotId = table.Column<int>(nullable: false),
                    CustomMessageForChatIcon = table.Column<string>(maxLength: 1500, nullable: true),
                    RasaBotUrl = table.Column<string>(maxLength: 1500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Widgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Widgets_WidgetThemes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "WidgetThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Widgets_WorkFlows_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "WorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WidgetId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    BrowserUserAgent = table.Column<string>(nullable: true),
                    CustomerIPAddress = table.Column<string>(nullable: true),
                    CreatedTimeUTC = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AgentRequestedTimeUTC = table.Column<DateTime>(nullable: true),
                    AgentAllocatedTimeUTC = table.Column<DateTime>(nullable: true),
                    AgentAcceptedTimeUTC = table.Column<DateTime>(nullable: true),
                    FirstAgentMessageTimeUTC = table.Column<DateTime>(nullable: true),
                    ClosingTimeUTC = table.Column<DateTime>(nullable: true),
                    ClosedByCustomer = table.Column<bool>(nullable: true),
                    CustomerId = table.Column<long>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    IsClosed = table.Column<byte>(nullable: false),
                    AssignedAgentName = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    ConversationTypeId = table.Column<int>(nullable: false),
                    AgentId = table.Column<int>(nullable: false),
                    BotFlowCompleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_ConversationStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ConversationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversations_Widgets_WidgetId",
                        column: x => x.WidgetId,
                        principalTable: "Widgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConversationHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    WidgetId = table.Column<int>(nullable: false),
                    ConversationId = table.Column<int>(nullable: false),
                    CreatedTimeUTC = table.Column<DateTime>(nullable: false),
                    FromStatus = table.Column<int>(nullable: false),
                    ToStatus = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationHistory_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationHistory_Widgets_WidgetId",
                        column: x => x.WidgetId,
                        principalTable: "Widgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConversationMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConversationId = table.Column<int>(nullable: false),
                    MessageTypeId = table.Column<int>(nullable: false),
                    AgentId = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreatedTimeUTC = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDelivered = table.Column<byte>(nullable: false),
                    ConversationTypeId = table.Column<int>(nullable: false),
                    AttachmentUrl = table.Column<string>(nullable: true),
                    AttachmentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationMessages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationMessages_ConversationMessageTypes_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalTable: "ConversationMessageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NavigationHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConversationId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Referrer = table.Column<string>(nullable: true),
                    LeavingTime = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavigationHistory_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationHistory_ConversationId",
                table: "ConversationHistory",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationHistory_WidgetId",
                table: "ConversationHistory",
                column: "WidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_ConversationId",
                table: "ConversationMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_MessageTypeId",
                table: "ConversationMessages",
                column: "MessageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_StatusId",
                table: "Conversations",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_WidgetId",
                table: "Conversations",
                column: "WidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_NavigationHistory_ConversationId",
                table: "NavigationHistory",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_ThemeId",
                table: "Widgets",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_WorkFlowId",
                table: "Widgets",
                column: "WorkFlowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatStatuses");

            migrationBuilder.DropTable(
                name: "ConversationHistory");

            migrationBuilder.DropTable(
                name: "ConversationInfo");

            migrationBuilder.DropTable(
                name: "ConversationMessages");

            migrationBuilder.DropTable(
                name: "CustomerIpInformation");

            migrationBuilder.DropTable(
                name: "MasterCustomers");

            migrationBuilder.DropTable(
                name: "NavigationHistory");

            migrationBuilder.DropTable(
                name: "WidgetAuthkeys");

            migrationBuilder.DropTable(
                name: "WidgetChannels");

            migrationBuilder.DropTable(
                name: "WidgetConversationTypes");

            migrationBuilder.DropTable(
                name: "ConversationMessageTypes");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "ConversationStatuses");

            migrationBuilder.DropTable(
                name: "Widgets");

            migrationBuilder.DropTable(
                name: "WidgetThemes");

            migrationBuilder.DropTable(
                name: "WorkFlows");
        }
    }
}
