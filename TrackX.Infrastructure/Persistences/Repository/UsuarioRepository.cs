using Microsoft.EntityFrameworkCore;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;

namespace TrackX.Infrastructure.Persistences.Repository
{
    public class UsuarioRepository : GenericRepository<TbUsuario>, IUsuarioRepository
    {
        private readonly DbCfContext _context;
        public UsuarioRepository(DbCfContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BaseEntityResponse<TbUsuario>> ListUsuarios(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<TbUsuario>();

            var usuarios = GetEntityQuery(x => x.Id != 0 && x.Correo != null)
                .AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        usuarios = usuarios.Where(x => x.Correo!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await usuarios.CountAsync();
            response.Items = await Ordering(filters, usuarios, !(bool)filters.Download!).ToListAsync();

            return response;
        }

        public async Task<TbUsuario> UserByEmail(string email)
        {
            var user = await _context.TbUsuarios.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Correo!.Equals(email));

            return user!;
        }
    }
}