namespace Domain.Entities;

public sealed class MusicianPaymentDetail
{
    public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Salary { get; set; }
    public decimal BasicSalary { get; set; } //Is the Musician basic salary in the payment day


    #region Navigation Properties

    public int MusicianId { get; set; }
    public Musician Musician { get; set; } = null!;

    public int RangePlusId { get; set; }
    public RangePlus RangePlus { get; set; } = null!;

    #endregion


}
