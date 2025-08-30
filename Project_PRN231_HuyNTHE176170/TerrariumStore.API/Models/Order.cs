using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TerrariumStore.API.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }  // Thay vì CustomerId
    [ForeignKey("UserId")]
    public User User { get; set; }  // Liên kết với Users

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = "Pending"; // "Pending", "Shipped", "Completed"

    [MaxLength(500)]
    public string ShippingAddress { get; set; }

    [MaxLength(100)]
    public string RecipientName { get; set; }

    [MaxLength(20)]
    public string RecipientPhone { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
