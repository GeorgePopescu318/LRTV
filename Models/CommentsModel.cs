namespace LRTV.Models;

public class CommentsModel
{
    public int Id { get; set; }
    public int userId { get; set; }
    public string userName { get; set; }
    public string text { get; set; }
    public DateTime postedDate { get; set; }
    public int newsId { get; set; }
    public virtual NewsModel? News { get; set; }
}
