
namespace AiWin_API
{
    public class JsonMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return Json.ToJson(this,false);
        }
    }
}
