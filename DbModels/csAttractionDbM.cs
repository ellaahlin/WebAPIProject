using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Linq;

using Configuration;
using Models;
using Models.DTO;
using System.Net;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;

//DbModels namespace is the layer which contains all the C# models of
//the database tables Select queries as well as results from a call to a View,
//Stored procedure, or Function.

//C# classes corresponds to table structure (no suffix) or
//specific search results (DTO suffix)
namespace DbModels;
[Table("Attractions", Schema = "supusr")]
[Index(nameof(AttractionName))]
public class csAttractionDbM : csAttraction, ISeed<csAttractionDbM>
{
    static public new string Hello { get; } = $"Hello from namespace {nameof(DbModels)}, class {nameof(csAttractionDbM)}";

    [Key]       // for EFC Code first
    public override Guid AttractionID { get; set; }

    [Required] // Required: AttractionName, Country, Category, Title and Description
    public override string AttractionName { get; set; }

    [Required]
    public override string Category { get; set; }

    [Required]
    public override string Title { get; set; }

    [Required]
    public override string Description { get; set; }

    //create own Foreign Key step 1
    [JsonIgnore]
    public virtual Guid CityID { get; set; }

    [NotMapped] //application layer can continue to read an Address without code change
    public override ICity City { get => CityDbM; set => new NotImplementedException(); }

    [JsonIgnore]
    [ForeignKey("CityID")]     //create own Foreign Key step 2
    public virtual csCityDbM CityDbM { get; set; } = null;

    #region correcting the 1st migration error
    [NotMapped] //application layer can continue to read a Comment without code change
    public override List<IComment> Comments { get => CommentsDbM?.ToList<IComment>(); set => new NotImplementedException(); }

    [JsonIgnore]     //create own Foreign Key step 2
    public virtual List<csCommentDbM> CommentsDbM { get; set; } = null;    //This is implemented in the database table

    #region randomly seed this instance
    public override csAttractionDbM Seed(csSeedGenerator sgen)
    {
        base.Seed(sgen);
        return this;
    }
    #endregion

    #region Update from DTO
    public csAttractionDbM UpdateFromDTO(csAttractionCUdto org)
    {
        AttractionName = org.AttractionName;
        Title = org.Title;
        Description = org.Description;
        Category = org.Category;

        return this;
    }
    #endregion

    #region constructors
    public csAttractionDbM() { }
    public csAttractionDbM(csAttractionCUdto org)
    {
        AttractionID = Guid.NewGuid();
        UpdateFromDTO(org);
    }
    #endregion

}

#endregion