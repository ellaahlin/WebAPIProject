using System.Collections.Generic;
using System.Net;
using Configuration;

//DbModels namespace is the layer which contains all the C# models of
//the database tables Select queries as well as results from a call to a View,
//Stored procedure, or Function.

//C# classes corresponds to table structure (no suffix) or
//specific search results (DTO suffix)
namespace Models;

public class csAttraction : IAttraction, ISeed<csAttraction>
{
    static public string Hello { get; } = $"Hello from namespace {nameof(Models)}, class {nameof(csAttraction)}";

    public virtual Guid AttractionID { get; set; }
    public virtual string AttractionName { get; set; }
    public virtual string Category { get; set; }
    public virtual string Title { get; set; }
    public virtual string Description { get; set; }

    public virtual ICity City { get; set; }


    //One Attraction can only have one city, title and description

    //One Attraction can have many Comments
    public virtual List<IComment> Comments { get; set; } = null;

    public string FullDescription => $"{Title}, {Category}, {AttractionName}, {Description}, {City}";

    public override string ToString()
    {
        var sRet = $"{FullDescription} [{AttractionID}]";

        if (City != null)
            sRet += $"\nLocation: {City}";


        if (Comments != null && Comments.Count > 0)
        {
            sRet += $"\n";
            foreach (var comment in Comments)
            {
                sRet += $"\n     {Comments}";
            }
        }
        else
            sRet += $"\n  - Has no comments";
        return sRet;
    }

    #region contructors
    public csAttraction() { }

    public csAttraction(csAttraction org)
    {
        this.Seeded = org.Seeded;
        this.AttractionID = org.AttractionID;
        this.AttractionName = org.AttractionName;
        this.Title = org.Title;
        this.Category = org.Category;

        //using Linq Select and copy contructor to create a list copy
        this.Comments = (org.Comments != null) ? org.Comments.Select(c => new csComment((csComment) c)).ToList<IComment>() : null;
        this.City = (org.City != null) ? new csCity((csCity)org.City) : null;
    }
    #endregion

    #region randomly seed this instance
    public bool Seeded { get; set; } = false;

    public virtual csAttraction Seed(csSeedGenerator sgen)
    {
        Seeded = true;
        AttractionID = Guid.NewGuid();
        AttractionName = sgen.AttractionNames;
        Category = sgen.Categories;
        Title = sgen.Title();
        Description = sgen.Description();

        return this;
    }
    #endregion
}

