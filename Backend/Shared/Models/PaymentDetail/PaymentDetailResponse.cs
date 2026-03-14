using System.Runtime.CompilerServices;

namespace Shared.Models.PaymentDetail;

public sealed class PaymentDetailResponse
{
    public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Salary { get; set; }
    public decimal BasicSalary { get; set; } 
    public int MusicianId { get; set; }
    public string MusicianName { get; set; } = null!;
    public int RangePlusId { get; set; }
}
