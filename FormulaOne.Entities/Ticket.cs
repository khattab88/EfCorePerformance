using System.ComponentModel.DataAnnotations.Schema;

namespace FormulaOne.Entities
{
    public class Ticket : BaseEntity
    {
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public double Price { get; set; }
        public string TicketLevel { get; set; } = string.Empty;
    }
}