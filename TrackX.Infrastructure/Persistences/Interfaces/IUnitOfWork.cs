﻿using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Declaracion o matricula de nuestra interfaces a nivel de repository

        IUsuarioRepository Usuario { get; }
        IGenericRepository<TbRol> Rol { get; }
        IGenericRepository<TbEmpleo> Empleo { get; }
        IGenericRepository<TbItinerario> Itinerario { get; }
        IGenericRepository<TbNoticia> Noticia { get; }
        IGenericRepository<TbWhs> Whs { get; }
        IGenericRepository<TbFinance> Finance { get; }
        IGenericRepository<TbExoneracion> Exoneracion { get; }
        IGenericRepository<TbLogs> Logs { get; }
        IGenericRepository<TbOrigen> Origen { get; }
        IGenericRepository<TbPol> Pol { get; }
        IGenericRepository<TbDestino> Destino { get; }
        IGenericRepository<TbPod> Pod { get; }
        IGenericRepository<TbControlInventarioWhs> ControlInventario { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}