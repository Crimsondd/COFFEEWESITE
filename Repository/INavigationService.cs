using DACS_DAMH.Models;

namespace DACS_DAMH.Repository
{
    public interface INavigationService
    {
        Task<IEnumerable<NavigationItemViewModel>> GetNavigationItemsAsync();
    }
}
