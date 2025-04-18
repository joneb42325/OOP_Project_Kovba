namespace OOP_Project_Kovba.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }

        public void SetUpdatedAt()
        {
            throw new NotImplementedException();
        }
        
        public virtual string GetInfo()
        {
            throw new NotImplementedException();
        }
    }
}
