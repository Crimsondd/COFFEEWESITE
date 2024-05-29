using DACS_DAMH.Models;
using DACS_DAMH.Repository;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DACS_DAMH.Repository
{
    public class NavigationService : INavigationService
    {
        private readonly ICategoryRepository _categoryRepository;

        public NavigationService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<NavigationItemViewModel>> GetNavigationItemsAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var rootItems = categories.Where(c => c.ParentId == null).Select(c => new NavigationItemViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Url = $"/Menu/Categoryproduct?categoryName={c.Name}", // Đường dẫn có thể thay đổi theo nhu cầu của bạn
                Children = GetChildren(categories, c.Id)
            }).ToList();

            return rootItems;
        }

        private List<NavigationItemViewModel> GetChildren(IEnumerable<Category> categories, int parentId)
        {
            var children = categories.Where(c => c.ParentId == parentId).Select(c => new NavigationItemViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Url = $"/Menu/Categoryproduct?categoryName={c.Name}", // Đường dẫn có thể thay đổi theo nhu cầu của bạn
                Children = GetChildren(categories, c.Id)
            }).ToList();

            return children;
        }
    }
}
