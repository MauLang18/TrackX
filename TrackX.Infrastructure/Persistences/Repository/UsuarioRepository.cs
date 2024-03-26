﻿using Microsoft.EntityFrameworkCore;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;

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
                .Where(x => x.Estado.Equals((int)StateTypes.Activo) && x.UsuarioEliminacionAuditoria == null && x.FechaEliminacionAuditoria == null)
                .FirstOrDefaultAsync(x => x.Correo!.Equals(email));

            return user!;
        }
    }
}