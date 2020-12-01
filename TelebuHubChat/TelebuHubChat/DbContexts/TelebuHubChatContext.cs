using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelebuHubChat.Models;

namespace TelebuHubChat.DbContexts
{
    public class TelebuHubChatContext:DbContext
    {
        public TelebuHubChatContext(DbContextOptions<TelebuHubChatContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder chatModelBuilder)
        {
            AddDefaultConstraints(chatModelBuilder);
        }

        public DbSet<WidgetThemes> WidgetThemes { get; set; }

        public DbSet<WorkFlows> WorkFlows { get; set; }

        public DbSet<Widgets> Widgets { get; set; }

        public DbSet<ConversationStatuses> ConversationStatuses { get; set; }

        public DbSet<Conversations> Conversations { get; set; }

        public DbSet<ConversationMessageTypes > ConversationMessageTypes { get; set; }

        public DbSet<ConversationMessages> ConversationMessages { get; set; }

        public DbSet<NavigationHistory> NavigationHistories { get; set; }
		
		 public DbSet<ChatStatuses> ChatStatuses { get; set; }

        public DbSet<ConversationHistory> ConversationHistory { get; set; }

	 public DbSet<WidgetAuthkeys> WidgetAuthkeys { get; set; }

        public DbSet<MasterCustomers> MasterCustomers { get; set; }

        public DbSet<ConversationInfo> ConversationInfo { get; set; }
        public DbSet<WidgetConversationTypes> WidgetConversationTypes { get; set; }
        public DbSet<WidgetChannels> WidgetChannels { get; set; }
	
        public DbSet<CustomerIpInformation> CustomerIpInformation { get; set; }

        public void AddDefaultConstraints(ModelBuilder chatModelBuilder)
        {

            //chatModelBuilder.Entity<Widgets>()
            //        .Property(p => p.IsActive)
            //        .HasDefaultValue(true);

            //chatModelBuilder.Entity<Widgets>()
            //        .Property(p => p.PopInAfterSeconds)
            //        .HasColumnType("tinyint")
            //        .HasDefaultValue(0);

        }
    }
}
