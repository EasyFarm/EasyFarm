namespace EasyFarm.Classes
{
    public class Result
    {
        public Result(string message)
        {
            Message = message;
        }

        public bool IsFailure { get; set; }

        public string Message { get; set; }

        public static Result Fail(string message)
        {
            return new Result(message)
            {
                IsFailure = true
            };
        }
    }
}