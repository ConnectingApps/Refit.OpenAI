using System.Collections.Generic;

namespace ConnectingApps.Refit.OpenAI.Variations.Response
{
    public class VariationResponse
    {
        public long Created { get; set; }
        public List<Variation> Data { get; set; }
    }
}
