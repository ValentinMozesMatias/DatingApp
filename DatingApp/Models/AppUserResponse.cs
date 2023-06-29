namespace DatingApp.Models
{
    public class AppUserResponse<T>

           where T : class
    {
        public AppUserResponse()
        {
            ErrorMessages = new List<string>();
        }
        public bool Success { get; set; }

        public List<string> ErrorMessages { get; set; }

        public T Data { get; set; }
    }
}
