
using System.Text.Json.Serialization;


namespace Rm.Model.Models
{
    public class BaseModel
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
