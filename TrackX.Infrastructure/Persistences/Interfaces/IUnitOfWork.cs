using TrackX.Infrastructure.FileStorage;

namespace TrackX.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Declaracion o matricula de nuestra interfaces a nivel de repository

        IUsuarioRepository Usuario { get; }
        IAzureStorage Storage { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}