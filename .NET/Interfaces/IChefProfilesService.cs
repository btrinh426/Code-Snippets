using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;

namespace Sabio.Services
{
    public interface IChefProfilesService
    {
        ChefProfile Get(int id);
        Paged<ChefProfile> GetAll(int pageIndex, int pageSize);
        Paged<ChefProfile> SearchChef(string searchString, int pageIndex, int pageSize);
        int Add(ChefProfileAddRequest model, int userId);
        void Update(ChefProfileUpdateRequest model, int userId);
        void Delete(int id);
        Paged<ChefProfile> GetCurrentUser(int userId, int pageIndex, int pageSize);
    }
}