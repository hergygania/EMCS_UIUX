using System.Threading.Tasks;

namespace WindowsServiceEbupot.Services
{
    public interface IApiService<TModel>
    {
        Task<PaginatedResponse<TModel>> GetAsync(PaginationParam paginationParam);

        Task<RequestResponse<TModel>> GetAsync(long id);

        Task<RequestResponse<TModel>> PostAsync(TModel model);

        Task<RequestResponse<TModel>> PutAsync(long id, TModel model);
    }
}
