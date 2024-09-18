namespace GameStore.Common
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public static OperationResult<T> SuccessResult(T data)
        {
            return new OperationResult<T> { Success = true, Data = data };
        }

        public static OperationResult<T> FailureResult(string errorMessage)
        {
            return new OperationResult<T> { Success = false, ErrorMessage = errorMessage };
        }
    }
}
