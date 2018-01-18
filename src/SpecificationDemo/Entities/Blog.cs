using System;
using System.Collections.Generic;

namespace SpecificationDemo.Entities
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Activated { get; set; }
        public DateTime? Removed { get; set; }
        public DateTime? Banned { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}