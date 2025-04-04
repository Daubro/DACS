using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Webbankhoahoconline.Repositories.Components
{
    public class InstructorsViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;
        
            public InstructorsViewComponent(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.Instructors.ToListAsync());

    }
}
