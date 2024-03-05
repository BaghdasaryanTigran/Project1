
using System.Text.Json.Serialization;
using Rm.Models;

namespace Rm.Model.Models;

public partial class Document : BaseModel
{

   
    public int CarId { get; set; }
  
    public int WorkerId { get; set; }

    [JsonIgnore]
    
    public virtual Car? Car { get; set; }

    [JsonIgnore]
    public virtual Worker? Worker { get; set; } 
}
public class DocumentResponse
{
    public int Id;
    public Car car { get; set; } = null!;
    public Worker worker { get; set; } = null!;
}
