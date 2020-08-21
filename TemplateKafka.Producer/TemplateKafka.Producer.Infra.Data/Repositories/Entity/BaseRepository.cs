using Microsoft.EntityFrameworkCore;
using TemplateKafka.Producer.Infra.Data.Context;

namespace TemplateKafka.Producer.Infra.Data.Repositories.Entity
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
    }
}
