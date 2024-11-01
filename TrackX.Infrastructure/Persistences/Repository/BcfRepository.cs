﻿using Microsoft.EntityFrameworkCore;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;

namespace TrackX.Infrastructure.Persistences.Repository;

public class BcfRepository : GenericRepository<TbBcf>, IBcfRepository
{
    private readonly DbCfContext _context;
    public BcfRepository(DbCfContext context) : base(context)
    {
        _context = context;
    }

    public async Task<string?> GetLastBcfAsync()
    {
        return await _context.TbBcfs
            .OrderByDescending(b => b.BCF)
            .Select(b => b.BCF)
            .FirstOrDefaultAsync();
    }
}