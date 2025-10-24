using OpenAI;
using OpenAI.Chat;


namespace Graduation_Project.Application.Services {
    public class AIService {
        private readonly OpenAIClient _openAiClient;
        private readonly string _connectionString;

        public AIService(string apiKey, string connectionString) {
            _openAiClient = new OpenAIClient(apiKey);
            _connectionString = connectionString;
        }

        public async Task<string> GenerateQueryAsync(string userQuestion, string schema) {
            var prompt = $"""
                         Given the following database schema:
                         {schema}
                         Convert this question into a SQL query:
                         Question: "{userQuestion}"
                         SQL Query:
                         i don`t you to response with any thing except the SQL query,
                         Hint : Avoid To get columns that have no meaning to user 
                         and have info that useful for developer 
                         i want to tell you some info that help you to generate correct query:
                         if the question contains 'doctors' , 'live coaches' 'Therapists' consider them as Service Providers 
                         and get from ServiceProviders table , 
                         also i want to give you some information about values columns in Service Providers table:
                         in Specialization column : if value = 0 in db revlect it as 'Doctors'
                         if value = 1 in db reflect it as 'Specialists'  
                         if value = 2 in db reflect it as 'SpeechTherapists'
                         if value = 3 in db reflect it as 'LifeCoaches'
                         if value = 4 in db reflect it as 'Therapists'
                         =============================================
                         also i want to give you some information about values columns in Sessions table:
                         in Type column : if value = 0 in db revlect it as 'Offline'
                         if value = 1 in db reflect it as 'Online'  
                         if value = 2 in db reflect it as 'Onlin/Offline' , 
                         in Status column : if value = 0 in db revlect it as 'NotStarted'
                         if value = 1 in db reflect it as 'Started'  
                         if value = 2 in db reflect it as 'InProgress'
                         if value = 3 in db reflect it as 'Posponed'
                         if value = 4 in db reflect it as 'Canceled'
                         if value = 5 in db reflect it as 'Finished' , 
                          in ReservationId column : if value = null in db reflect it as 'Free Session' other with "Reserved Session"
                         """;

            var response = await _openAiClient.ChatEndpoint.GetCompletionAsync(
                new ChatRequest(
                    messages: new List<Message> { new Message(Role.User, prompt) },
                    model: "gpt-4o"
                ));


            return CleanSqlString(response.Choices[0].Message.Content.ToString());
        }

        public async Task<string> GenerateResponseAsync(string userQuestion, string queryResults) {
            var prompt = $"""
                        Question: "{userQuestion}"
                        Database Results: {queryResults}
                        Summarize this in a user-friendly way:
                        """;

            var response = await _openAiClient.ChatEndpoint.GetCompletionAsync(
                new ChatRequest(
                    messages: new List<Message> { new Message(Role.User, prompt) },
                    model: "gpt-4o"
                ));

            return response.Choices[0].Message.Content.ToString();
        }

        public static string CleanSqlString(string raw) {
            if (string.IsNullOrWhiteSpace(raw)) {
                return string.Empty;
            }

            raw = raw.Trim();

            if (raw.StartsWith("```")) {
                int firstNewLine = raw.IndexOf('\n');
                if (firstNewLine >= 0) {
                    raw = raw.Substring(firstNewLine + 1);
                }
            }

            if (raw.EndsWith("```")) {
                raw = raw.Substring(0, raw.Length - 3);
            }



            return raw.Trim();
        }



    }
}
