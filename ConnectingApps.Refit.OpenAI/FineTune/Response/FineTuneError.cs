namespace ConnectingApps.Refit.OpenAI.FineTune.Response
{
    public class FineTuneError
    {
        public string Code { get; set; } = null!;    
        public string Param { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
