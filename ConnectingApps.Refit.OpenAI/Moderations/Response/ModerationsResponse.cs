namespace ConnectingApps.Refit.OpenAI.Moderations.Response
{
    public class ModerationsResponse
    {
        public string Id { get; set; } = null!;
        public string Model { get; set; } = null!;
        public Result[] Results { get; set; } = null!;
    }
}
