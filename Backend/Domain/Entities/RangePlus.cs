namespace Domain.Entities;

public sealed class RangePlus
{    
    public int Id { get; set; }
    public int MinExperience { get; set; }
    public int MaxExperience { get; set; }
    public decimal Plus { get; set; }

    #region Navigation Properties
    public ICollection<MusicianPaymentDetail> MusicianPaymentDetails { get; set; } = new List<MusicianPaymentDetail>();

    #endregion
}
