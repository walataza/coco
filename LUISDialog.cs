using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoCoChatbot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;

namespace CoCoChatbot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var luisAppId = "d8de17d9-f65c-40f5-8a3e-9009ea6910f1";
            var predictionKey = "c2915c46947441088caec6277af3d6d8";
            var endpoint = "https://terabhyte.com/clients/coco/api/v2.0";
            var credentials = new ApiKeyServiceClientCredentials(predictionKey);
            var client = new LUISRuntimeClient(credentials) { Endpoint = endpoint };

            string userInput = "I want to book a suite for tomorrow.";
            var predictionRequest = new PredictionRequest { Query = userInput };

            try
            {
                var predictionResponse = await client.Prediction.GetSlotPredictionAsync(luisAppId, "production", predictionRequest);
                var topIntent = predictionResponse.Prediction.TopIntent;
                var entities = predictionResponse.Prediction.Entities;

                if (topIntent == "BookRoom")
                {
                    string roomType = entities.ContainsKey("RoomType") ? entities["RoomType"][0] : "any room";
                    Console.WriteLine($"You want to book a {roomType}.");
                }
                else if (topIntent == "CheckAvailability")
                {
                    string roomType = entities.ContainsKey("RoomType") ? entities["RoomType"][0] : "any room";
                    Console.WriteLine($"You want to check availability for a {roomType}.");
                }
                else
                {
                    Console.WriteLine("I'm sorry, I didn't understand your request.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
