using System;

namespace Core.Infrastructure
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public BaseEntity()
        {
            Created = DateTime.UtcNow;
            LastModified = DateTime.Now;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}


