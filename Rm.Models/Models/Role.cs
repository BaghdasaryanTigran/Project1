

namespace Rm.Model.Models;

public partial class Role
{
    public short Id { get; set; }
    
    public string Role1 { get; set;  } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
