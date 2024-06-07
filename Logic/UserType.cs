namespace LRTV.Logic
{
    public enum UserType
    {
        //Visitor = 0,//just view permission, cant add comments or open forum topics
        Member = 0,//can comment and open forum topics
        Moderator = 1,//can add, remove news, same for players and can ban members from the forum discussions-and close said forums
        Admin = 2//can upgrade members to moderator role and remove said role
    }
}
