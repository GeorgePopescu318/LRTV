using LRTV.Models;

namespace LRTV.ViewModels;

public class News_CommentsViewModel
{
    public NewsModel News { get; set; }
    public List<CommentsModel> Comments { get; set; }
}
