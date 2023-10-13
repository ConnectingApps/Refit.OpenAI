namespace ConnectingApps.Refit.OpenAI.Moderations.Response
{
    public class Result
    {
        public bool Flagged { get; set; }
        public Categories Categories { get; set; } = null!;
        public CategoryScores CategoryScores { get; set; } = null!;
    }
}
