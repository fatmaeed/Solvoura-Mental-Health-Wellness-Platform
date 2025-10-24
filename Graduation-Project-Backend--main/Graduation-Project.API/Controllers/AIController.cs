using System.Threading.Tasks;
using Graduation_Project.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly AIService _aiService;
        private readonly AIDatabaseService _dbService;

        public AIController(AIService aiService, AIDatabaseService dbService)
        {
            _aiService = aiService;
            _dbService = dbService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] string userQuestion)
        {
       
            var sqlQuery = await _aiService.GenerateQueryAsync(userQuestion , GetDatabaseSchemaString());

            var queryResults = await _dbService.ExecuteQueryAsync(sqlQuery);

            var aiResponse = await _aiService.GenerateResponseAsync(userQuestion, queryResults);

            return Ok(new { Question = userQuestion, Answer = aiResponse });
        }

        [NonAction]
        public string GetDatabaseSchemaString()
        {
            string query = @"
                             SELECT 
                                 t.TABLE_NAME,
                                 c.COLUMN_NAME,
                                 c.DATA_TYPE
                             FROM 
                                 INFORMATION_SCHEMA.TABLES t
                             JOIN 
                                 INFORMATION_SCHEMA.COLUMNS c 
                                 ON t.TABLE_NAME = c.TABLE_NAME
                             WHERE 
                                 t.TABLE_TYPE = 'BASE TABLE'
                             ORDER BY 
                                 t.TABLE_NAME, c.ORDINAL_POSITION";
            var schemaString =  _dbService.ExecuteQueryAsync(query).Result;

                
            return schemaString;
        }
    }
}

                