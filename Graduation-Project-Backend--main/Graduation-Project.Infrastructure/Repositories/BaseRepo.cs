using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Interfaces.IEntities;
using Graduation_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Infrastructure.Repositories {

    public class BaseRepo<Entity> : IBaseRepo<Entity> where Entity : class, IBaseEntity {
        protected readonly MentalDbContext mentalDbContext;

        public BaseRepo(MentalDbContext mentalDbContext) {
            this.mentalDbContext = mentalDbContext;
        }

        public Entity Add(Entity entity) {
            return mentalDbContext.Set<Entity>().Add(entity).Entity;
        }

        public Entity? Delete(int id) {
            Entity? entity = mentalDbContext.Set<Entity>().Find(id);
            if (entity == null) {
                return null;
            }
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
            return entity;
        }

        public Entity? Get(int id) {
            return mentalDbContext.Set<Entity>().Find(id);
        }

        public IEnumerable<Entity> GetAll() {
            return mentalDbContext.Set<Entity>().Where(x => !x.IsDeleted).ToList();
        }

        public IQueryable<Entity> GetAllAsQuerable() {
            return mentalDbContext.Set<Entity>().Where(x => !x.IsDeleted);
        }

        public Entity Update(Entity entity) {
            mentalDbContext.Entry(entity).State = EntityState.Modified;
            entity.UpdatedAt = DateTime.Now;
            return entity;
        }
    }
}