using System;
namespace Models
{
    public interface IAttraction { 
        public Guid AttractionID { get; set; }
        public string AttractionName { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // One attraction can have many different comments
        public List<IComment> Comments { get; set; }

        public ICity City { get; set; }
     
    }
}

