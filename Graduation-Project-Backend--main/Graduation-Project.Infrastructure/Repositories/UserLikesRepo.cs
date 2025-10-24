using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories
{
    public class UserLikesRepo(MentalDbContext mentalDbContext) : BaseRepo<UserLikes>(mentalDbContext), IUserLikesRepo
    {
    }
}
