using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories {

    internal class SymptomRepo : BaseRepo<SymptomEntity>, ISymptomRepo {

        public SymptomRepo(MentalDbContext mentalDbContext) : base(mentalDbContext) {
        }
    }
}