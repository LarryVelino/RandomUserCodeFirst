namespace Lib.Users.Domain
{
    public class ResultDto<T>
    {
        public bool Success;

        public T Results { get; set; }
    }
}