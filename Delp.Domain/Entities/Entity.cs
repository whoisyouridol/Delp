using System.ComponentModel.DataAnnotations;

namespace Delp.Domain.Entities
{
    public class Entity<T>
    {
        [Key]
        public T Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
