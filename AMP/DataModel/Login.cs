namespace Anfema.Amp.DataModel
{
    public class Login
    {
        public string token{ get; set; }
        public int? user { get; set; } // is nullable because of an AMP error...
        public string api_url { get; set; }
        public string version { get; set; }
    }
}
