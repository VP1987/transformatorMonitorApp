namespace TransformerMonitor.Domain.Entities;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan ShiftStart { get; set; }
    public TimeSpan ShiftEnd { get; set; }
    public bool IsOnShiftToday { get; set; }
    public ICollection<Technician> Technicians { get; set; } = new List<Technician>();
    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
}
