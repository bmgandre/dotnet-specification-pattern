using System;

namespace SpecificationDemo.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Activated { get; set; }
        public DateTime? Removed { get; set; }
        public DateTime? Banned { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}