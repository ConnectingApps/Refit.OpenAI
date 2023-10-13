namespace ConnectingApps.Refit.OpenAI.Moderations.Response
{
    public class CategoryScores
    {
        public double Sexual { get; set; }
        public double Hate { get; set; }
        public double Harassment { get; set; }
        public double Selfharm { get; set; }
        public double Sexualminors { get; set; }
        public double Hatethreatening { get; set; }
        public double Violencegraphic { get; set; }
        public double Selfharmintent { get; set; }
        public double Selfharminstructions { get; set; }
        public double Harassmentthreatening { get; set; }
        public double Violence { get; set; }
    }
}
