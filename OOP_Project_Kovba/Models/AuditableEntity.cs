namespace OOP_Project_Kovba.Models
{
    public abstract class AuditableEntity
    {
        private DateTime _createdAt;
        private DateTime _updatedAt;    
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
