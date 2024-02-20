namespace Core.Entities
{
    // BaseEntity class provides a foundational structure for entities within the application by defining a common Id property
    //This promotes code reuse, encapsulates common functionality, and ensures consistency across entity classes
    public class BaseEntity
    {
        public int Id { get; set; }
    }
}