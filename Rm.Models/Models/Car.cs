

namespace Rm.Model.Models;

public partial class Car : BaseModel
{
    
    public string Name { get; set; } = null!;

    public string Number { get; set; } = null!;

   // public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
