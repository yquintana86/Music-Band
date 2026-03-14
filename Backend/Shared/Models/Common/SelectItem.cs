namespace SharedLib.Models.Common;

public class SelectItem
{
    public string Id { get; set; } = null!;

    public string Text { get; set; } = null!;

    public SelectItem Clone()
    {
        return new SelectItem { Id = this.Id, Text = this.Text };
    }
}
