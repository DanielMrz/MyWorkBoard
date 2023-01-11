using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Tables;

namespace MyWorkBoard.Entities.Config
{
    public class StateConfig : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.Property(x => x.StateValue).IsRequired().HasMaxLength(60);
            // Określam jakie dane początkowe powinna mieć konkretna tabela
            builder.HasData(new State() { Id = 1, StateValue = "To Do" },
                            new State() { Id = 2, StateValue = "Doing" },
                            new State() { Id = 3, StateValue = "Done" },
                            new State() { Id = 4, StateValue = "On Hold" },
                            new State() { Id = 5, StateValue = "Rejected" });
        }
    }
}
