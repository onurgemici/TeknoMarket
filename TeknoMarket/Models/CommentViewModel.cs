namespace TeknoMarket.Models;

public class CommentViewModel
{
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; }
}
