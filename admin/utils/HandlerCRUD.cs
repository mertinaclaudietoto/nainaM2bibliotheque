using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Linq.Expressions;

    public class HandlerCRUD<T> where T : class
    {
        private readonly  ApplicationDbContext _context;
        private readonly string _tablename="";
        
        public HandlerCRUD( ApplicationDbContext context,string tablename)
        {
            _context = context;
            _tablename=tablename;
        }
        public PropertyInfo getContext()
        {
             var property = _context.GetType().GetProperty(_tablename);
            if (property == null)
                throw new Exception($"Le DbSet '{_tablename}' n'existe pas dans le DbContext.");
            return property;
        }

        public async Task<List<T>> getAll()
        {
           var property=getContext();
            var dbSetObject = property.GetValue(_context);
            var queryable = dbSetObject as IQueryable<T>;
            if (queryable == null)
                throw new Exception($"Le DbSet '{_tablename}' n'est pas du type IQueryable<{typeof(T).Name}>.");
            return await queryable.ToListAsync();
        }
        public async Task<T> SaveAsync<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> SaveAsync(T entity)
        {
            var property=getContext();
            var dbSetObject = property.GetValue(_context);
            
            var dbSet = dbSetObject! as dynamic;
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            var property=getContext();

            var dbSetObject = property.GetValue(_context);
            var dbSet = dbSetObject! as dynamic;

            dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteAsync(T entity)
        {
            var property=getContext();
            var dbSetObject = property.GetValue(_context);
            var dbSet = dbSetObject! as dynamic;

            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public ResponseApi<T> notNullvalue()
        {
            var errorResponse = new ResponseApi<T>.Builder()
            .SetSuccess(false)
            .SetMessage("Données vide")
            .SetData(null)
            .Build();
            return errorResponse;
        }
       public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, string search = null)
        {
            var property=getContext();
            var dbSetObject = property.GetValue(_context);
            var dbSet = dbSetObject as IQueryable<T>;
            if (dbSet == null)
                throw new Exception("Impossible de convertir le DbSet en IQueryable.");

            if (!string.IsNullOrEmpty(search))
            {
                dbSet = dbSet.Where(e => EF.Functions.Like(EF.Property<string>(e, "Name"), $"%{search}%"));
            }
            int skip = (pageNumber - 1) * pageSize;
            var items = await dbSet
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return items;
        }

        public async Task<int> GetNbrLigneAsync()
        {
            var property=getContext();
            var dbSetObject = property.GetValue(_context);
            var dbSet = dbSetObject! as IQueryable<T>;
            int totalCount = await dbSet.CountAsync();
            return totalCount;
        }
        public async Task<List<T>> SelectByColumnAsync<T>(string columnName,object value)
        {
            var property=getContext();
            var dbSetObject = property.GetValue(_context);
            if (dbSetObject == null)
                throw new Exception($"Impossible de récupérer la table '{_tablename}'.");
            IQueryable<T> dbSet = dbSetObject as IQueryable<T>
                ?? throw new Exception($"Le DbSet '{_tablename}' n'est pas du type IQueryable<{typeof(T).Name}>.");

            var classProperty = typeof(T).GetProperty(columnName);
            if (classProperty == null)
                throw new Exception(
                    $"La colonne '{columnName}' n'existe pas dans {typeof(T).Name}");
            // x => declaration de T entenque x
            var parameter = Expression.Parameter(typeof(T), "x");
            // x.ColumnName
            var propertyAccess = Expression.MakeMemberAccess(parameter, classProperty);
            // // valeur
            // var constant =Expression.Constant(value, classProperty.PropertyType); // forcer le type de la constante à correspondre à celui de la propriété 
            // // x.ColumnName == value
            // var equality = Expression.Equal(propertyAccess, constant);
            Expression equality;
            if (classProperty.PropertyType == typeof(string))
            {
                // Méthode Trim()
                var trimMethod = typeof(string).GetMethod("Trim", Type.EmptyTypes);
                var left = Expression.Call(propertyAccess, trimMethod);

                var right = Expression.Constant(value?.ToString()?.Trim(), typeof(string));

                equality = Expression.Equal(left, right);
            }
            else
            {
                // Autres types
                var constant = Expression.Constant(value, classProperty.PropertyType);
                equality = Expression.Equal(propertyAccess, constant);
            }
            // x => x.ColumnName == value
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);
            // SELECT * FROM TEntity WHERE ColumnName = value
            return await dbSet.Where(lambda).ToListAsync();
        }
        public async Task<T?> SelectOneByColumnAsync<T>(string columnName,object value) 
        {
            var list = await SelectByColumnAsync<T>(columnName, value);
            return list.FirstOrDefault();
        }


    }