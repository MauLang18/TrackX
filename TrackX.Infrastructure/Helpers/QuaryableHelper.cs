using TrackX.Infrastructure.Commons.Bases.Request;

namespace TrackX.Infrastructure.Helpers
{
    public static class QuaryableHelper
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, BasePaginationRequest request)
        {
            return queryable.Skip((request.NumPage - 1) * request.Records).Take(request.Records);
        }
    }
}