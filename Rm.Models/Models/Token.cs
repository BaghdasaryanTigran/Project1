

namespace Rm.Model.Models;

public partial class Token
{
    public string Token1 { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public int UserId { get; set; }

    public int Id { get; set; }

    public virtual User User { get; set; } = null!;
}
