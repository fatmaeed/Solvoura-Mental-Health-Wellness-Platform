using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories {

    public class ClientRepo(MentalDbContext mentalDbContext) : BaseRepo<ClientEntity>(mentalDbContext), IClientRepo {
    }
}