using System.ComponentModel.DataAnnotations;
using Models.Util;

namespace Models.Models;

public class Order
{
    [Key]
    public required Guid Id { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public List<Box> Boxes { get; set; } = [];
    
    public required Customer Customer { get; set; }
    
    public required ShippingStatus ShippingStatus { get; set; }
   
    [PositiveNumber]
    public required float TotalPrice { get; set; }
    
    [PositiveNumber]
    public required int TotalBoxes { get; set; }
}