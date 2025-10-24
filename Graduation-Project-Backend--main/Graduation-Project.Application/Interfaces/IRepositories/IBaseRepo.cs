using Graduation_Project.Domain.Interfaces.IEntities;

namespace Graduation_Project.Application.Interfaces.IRepositories {

    public interface IBaseRepo<Entity> where Entity : IBaseEntity {

        Entity Add(Entity entity);

        Entity Update(Entity entity);

        Entity? Delete(int id);

        Entity? Get(int id);

        IEnumerable<Entity> GetAll();

        public IQueryable<Entity> GetAllAsQuerable();
    }
}