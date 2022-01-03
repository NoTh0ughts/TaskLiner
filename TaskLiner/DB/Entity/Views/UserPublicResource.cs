namespace TaskLiner.DB.Entity.Views
{
    public class UserPublicResource
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Proffesion { get; set; }
    }

    public static partial class ToResourceHelper
    {
        public static UserPublicResource ToPublicResource(this User user)
        {
            return new UserPublicResource
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Nickname = user.Nickname,
                Email = user.Email,
                Avatar = user.Avatar,
                Proffesion = user.Proffesion,
            };
        }
    }
}
