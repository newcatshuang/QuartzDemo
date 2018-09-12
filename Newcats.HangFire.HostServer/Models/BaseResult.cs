namespace Newcats.HangFire.HostServer.Models
{
    public class BaseResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public BaseResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public BaseResult(int code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
