using LRTV.Logic;

namespace LRTV.ViewModels
{
    public class CreateUserViewModel
    {

        public int Id { get; set; }
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? PasswordConfirm { get; set; }

        public UserType Role { get; set; }

    }
}
