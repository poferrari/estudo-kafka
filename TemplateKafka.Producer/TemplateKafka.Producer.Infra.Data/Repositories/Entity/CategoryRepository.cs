﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Entities;
using TemplateKafka.Producer.Domain.Products.Repositories;
using TemplateKafka.Producer.Infra.Data.Context;

namespace TemplateKafka.Producer.Infra.Data.Repositories.Entity
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext context)
            : base(context) { }

        public async Task<IEnumerable<Category>> GetCategories()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<Category> GetCategory(Guid id)
            => await _dbSet.FindAsync(id);

        public async Task<bool> InsertCategories(IEnumerable<Category> categories)
        {
            await _dbSet.AddRangeAsync(categories);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
