using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories {

    public class PrescriptionRepo(MentalDbContext context) : BaseRepo<PrescriptionEntity>(context), IPrescriptionRepo {
    }
}