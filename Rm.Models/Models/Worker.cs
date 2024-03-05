using System;
using System.Collections.Generic;
using Rm.Model.Models;

namespace Rm.Models;

public partial class Worker : BaseModel
{


    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Position { get; set; } = null!;

    //public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
