namespace ForkliftAPI.Domain.Entities
{
    public class Forklift
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string ModelNumber { get; private set; } = null!;
        public DateTime ManufacturingDate { get; private set; }

        // Constructor for creating new forklift
        public Forklift(string name, string modelNumber, string manufacturingDate)
        {
            Name = name;
            ModelNumber = modelNumber;
            ManufacturingDate = DateTime.Parse(manufacturingDate);
        }

        // EF Core requires parameterless constructor for hydration
        public Forklift() { }
    }
}