namespace LRTV.Models;

public class NewsModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Lead { get; set; }
    public string Body { get; set; }
    public string Author { get; set; }

    public DateTime Data { get; set; }

    public int CathegoryID { get; set; }

    public CathegoryModel? Cathegory { get; set; }

    public string? Image {  get; set; }

    public virtual ICollection<CommentsModel> Comments { get; set; }
}
