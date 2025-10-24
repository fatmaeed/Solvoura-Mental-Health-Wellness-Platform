using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Interfaces.IServices
{
    public interface IAIService
    {
        Task<string> ClassifyQuestionAsync(string question);
        Task<string> GenerateSQLAsync(string question, string schema);
        Task<string> GetAnswerFromResultsAsync(string question, string resultJson);
    }
}
