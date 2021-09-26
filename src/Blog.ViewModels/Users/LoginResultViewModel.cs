namespace Blog.ViewModels.Users
{
    public class LoginResultViewModel
    {
        public string Id { get; set;}

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string Password { get; set; }
    }
}