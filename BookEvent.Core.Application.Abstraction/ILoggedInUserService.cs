namespace BookEvent.Core.Application.Abstraction
{
    public interface ILoggedInUserService
    {
        public string? UserId { get; set; }
        public string? FullName { get; set; }

    }
}
