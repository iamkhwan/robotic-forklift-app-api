namespace ForkliftAPI.Domain.Entities
{
    public class ForkliftCommand
    {
        public int Id { get; private set; }
        public string ModelNumber { get; private set; } = null!;
        public string Command { get; private set; } = null!;
        public DateTime ActionDate { get; private set; }

        // Constructor for creating new forklift
        public ForkliftCommand(string modelNumber, string command, string actionDate)
        {
            ModelNumber = modelNumber;
            Command = command;
            ActionDate = DateTime.Parse(actionDate);
        }

        // EF Core requires parameterless constructor for hydration
        public ForkliftCommand() { }
    }
}