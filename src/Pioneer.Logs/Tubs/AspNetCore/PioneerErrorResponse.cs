using Newtonsoft.Json;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    public class PioneerErrorResponse
    {
        public string ErrorId { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
