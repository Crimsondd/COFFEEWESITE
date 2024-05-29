using DACS_DAMH.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DACS_DAMH.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly INavigationService _navigationService;

        public NavigationMenuViewComponent(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var navigationItems = await _navigationService.GetNavigationItemsAsync();
            return View(navigationItems);
        }
    }
}
