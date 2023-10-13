using ConnectingApps.Refit.OpenAI.Moderations.Response;
using System.Text.Json;
using FluentAssertions;

namespace ConnectingApps.Refit.OpenAI.Test
{
    public class ModerationsResponseTest
    {
        [Theory]
        [InlineData("""
                    {
                      "id": "modr-89CCHcVvSKTnHSuF8VRtmMXLfBtcA",
                      "model": "text-moderation-006",
                      "results": [
                        {
                          "flagged": false,
                          "categories": {
                            "sexual": false,
                            "hate": false,
                            "harassment": false,
                            "self-harm": false,
                            "sexual/minors": false,
                            "hate/threatening": false,
                            "violence/graphic": false,
                            "self-harm/intent": false,
                            "self-harm/instructions": false,
                            "harassment/threatening": false,
                            "violence": true
                          },
                          "category_scores": {
                            "sexual": 3.6963891034247354e-05,
                            "hate": 0.0001245409803232178,
                            "harassment": 0.009186690673232079,
                            "self-harm": 9.155368752544746e-05,
                            "sexual/minors": 2.1913865566602908e-07,
                            "hate/threatening": 1.2790572327503469e-05,
                            "violence/graphic": 4.758815703098662e-05,
                            "self-harm/intent": 8.181822340702638e-05,
                            "self-harm/instructions": 2.0860365518293733e-11,
                            "harassment/threatening": 0.013410148210823536,
                            "violence": 0.9223754405975342
                          }
                        }
                      ]
                    }
                    """)]
        public void TestDeSerializeModerationsResponse(string jsonString)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseJsonNamingPolicy()
            };

            // Deserialize
            ModerationsResponse response = JsonSerializer.Deserialize<ModerationsResponse>(jsonString, jsonOptions)!;
            response.Should().NotBeNull();
            response.Id.Should().Be("modr-89CCHcVvSKTnHSuF8VRtmMXLfBtcA");
            response.Model.Should().Be("text-moderation-006");
            response.Results.Should().NotBeNull();
            response.Results.Length.Should().Be(1);
            response.Results[0].Flagged.Should().BeFalse();
            response.Results[0].Categories.Should().NotBeNull();
            response.Results[0].CategoryScores.Harassmentthreatening.Should().BeGreaterThan(0.01341);
            response.Results[0].CategoryScores.Selfharmintent.Should().BeGreaterThan(0.00008180);
        }
    }
}