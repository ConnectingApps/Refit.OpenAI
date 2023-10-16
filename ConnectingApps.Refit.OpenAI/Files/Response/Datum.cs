using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingApps.Refit.OpenAI.Files.Response
{
    public class Datum
    {
        public string Object { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public string Filename { get; set; } = null!;
        public int Bytes { get; set; }
        public int CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public object StatusDetails { get; set; } = null!;
    }
}
