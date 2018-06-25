using CheeseMVC.Data;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if(ModelState.IsValid)
            {
                Menu newMenu = new Menu();
                newMenu.Name = addMenuViewModel.Name;
                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }
            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);

            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel();
            viewMenuViewModel.Menu = menu;
            viewMenuViewModel.Items = items;

            return View(viewMenuViewModel);
        }


        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(m => m.ID == id);
            List<Cheese> cheeses = context.Cheeses.ToList();

            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);
            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if(ModelState.IsValid)
            {
                int menuID = addMenuItemViewModel.menuID;
                int cheeseID = addMenuItemViewModel.cheeseID;

                IList<CheeseMenu> existingItems = context.CheeseMenus.Where(cm => cm.MenuID == menuID)
                    .Where(cm => cm.CheeseID == cheeseID).ToList();

                if(existingItems.Count == 0)
                {
                    CheeseMenu newCheeseMenu = new CheeseMenu();
                    newCheeseMenu.CheeseID = cheeseID;
                    newCheeseMenu.MenuID = menuID;

                    context.CheeseMenus.Add(newCheeseMenu);
                    context.SaveChanges();
                }

                return Redirect("/Menu/ViewMenu/" + menuID);
            }

            return View(addMenuItemViewModel);
        }

    }
}
