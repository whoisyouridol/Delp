using Delp.Domain.Enums;

namespace Delp.Domain.Entities
{
    public class Animal : Entity<int>
    {
        public string Name { get; set; }
        public string Breed { get; set; }
        public AnimalType Type { get; set; }
        public string Description { get; set; }
        public string Health { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
