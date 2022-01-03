namespace TaskLiner.DB.Entity.Views
{
    public class UserWithToken : User
    {
        public string Token { get; set; }

        public UserWithToken()
        {
                
        }
        public UserWithToken(User user, string token)
        {
            Id = user.Id;
            Fullname = user.Fullname;
            Nickname = user.Nickname;
            Email = user.Email;
            Avatar = user.Avatar;
            Proffesion = user.Proffesion;
            Password = user.Password;
            Token = token;
        }
    }
}