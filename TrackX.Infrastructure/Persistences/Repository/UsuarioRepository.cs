using Microsoft.EntityFrameworkCore;
using TrackX.Domain.Entities;
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

        public async Task<TbUsuario> UserByEmail(string email)
        {
            var user = await _context.TbUsuarios.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Correo!.Equals(email));

            return user!;
        }
    }
}