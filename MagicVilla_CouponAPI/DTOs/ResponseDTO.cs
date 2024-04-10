using System.Net;

namespace MagicVilla_CouponAPI.DTOs
{
    public class ResponseDTO
    {
        public ResponseDTO()
        {
            ErrorMessages = [];
        }

        public bool IsSuccess { get; set; }
        public object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
