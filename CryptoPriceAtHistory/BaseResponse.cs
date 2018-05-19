using CryptoPriceAtHistory.Enums;

namespace CryptoPriceAtHistory
{
    public abstract class BaseResponse
    {
        public bool IsSuccess { get; protected set; }
        public string ErrorMessage { get; protected set; }
        public ErrorType? Error { get; protected set; }

        protected BaseResponse(bool isSuccess, string errorMessage = null, ErrorType? errorType = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Error = errorType;
        }
    }
}
