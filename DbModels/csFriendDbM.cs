using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Linq;

using Configuration;
using Models;
using Models.DTO;

//DbModels namespace is the layer which contains all the C# models of
//the database tables Select queries as well as results from a call to a View,
//Stored procedure, or Function.

//C# classes corresponds to table structure (no suffix) or
//specific search results (DTO suffix)
namespace DbModels;

public class csFriendDbM : csFriend, ISeed<csFriendDbM>
{
    static public new string Hello { get; } = $"Hello from namespace {nameof(DbModels)}, class {nameof(csFriendDbM)}";

    [Key]       // for EFC Code first
    public override Guid FriendId { get; set; }

    #region randomly seed this instance
    public override csFriendDbM Seed(csSeedGenerator sgen)
    {
        base.Seed(sgen);
        return this;
    }
    #endregion

    #region Update from DTO
    public csFriendDbM UpdateFromDTO(csFriendCUdto org)
    {
        FirstName = org.FirstName;
        LastName = org.LastName;
        Birthday = org.Birthday;

        return this;
    }
    #endregion

    #region constructors
    public csFriendDbM() { }
    public csFriendDbM(csFriendCUdto org)
    {
        FriendId = Guid.NewGuid();
        UpdateFromDTO(org);
    }
    #endregion

}

