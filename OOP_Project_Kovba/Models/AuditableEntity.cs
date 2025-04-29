namespace OOP_Project_Kovba.Models
{
    public abstract class AuditableEntity
    {
        private DateTime _createdAt;
        private DateTime _updatedAt;    
        public DateTime CreatedAt
        {
            get => _createdAt;
            set 
            {
                _createdAt = value;
            }
        }
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
            }
        }

        public AuditableEntity() {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.Now;
        }

        public abstract string GetInfo();
    }
}
