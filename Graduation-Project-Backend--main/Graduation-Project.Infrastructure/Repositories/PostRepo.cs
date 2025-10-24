using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Infrastructure.Repositories
{
    public class PostRepo : BaseRepo<PostEntity>, IPostRepo
    {
        public PostRepo(MentalDbContext mentalDbContext) : base(mentalDbContext)
        {
        }
    }
}
