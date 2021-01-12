using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Bank;
using SMP.Models.Grada;
using SMP.Models.Kompania;
using SMP.Models.Paga;
using SMP.Models.Punetori;
using SMP.Models.Raport;
using SMP.ViewModels.Raport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Controllers
{
    public class RaportController : BaseController
    {
        private IKompaniaRepository kompaniaRepository;
        private IPunetoriRepository punetoriRepository;
        private IPagaRepository pagaRepository;
        private IGradaRepository gradaRepository;
        private IBankRepository bankRepository;
        private IRaportRepository raportRepository;
        private ISession session;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RaportController(IKompaniaRepository _kompaniaRepository, IPunetoriRepository _punetoriRepository, IPagaRepository _pagaRepository,
            IGradaRepository _gradaRepository, IBankRepository _bankRepository, IRaportRepository _raportRepository, IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _webHostEnvironment,
            RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager, AlertService _alertService) 
            : base(_roleManager, _userManager)
        {
            kompaniaRepository = _kompaniaRepository;
            punetoriRepository = _punetoriRepository;
            pagaRepository = _pagaRepository;
            gradaRepository = _gradaRepository;
            bankRepository = _bankRepository;
            raportRepository = _raportRepository;
            httpContextAccessor = _httpContextAccessor;
            session = httpContextAccessor.HttpContext.Session;
            webHostEnvironment = _webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        // GET: RaportController
        public async Task<ActionResult> IndexAsync()
        {
            string role = User.IsInRole("HR") ? "HR" : "Administrator";

            ViewBag.RaportiId = EnumsToSelectList<Raportet>.GetSelectList();
            ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectListBasedOnRole(role, user.KompaniaId);
            ViewBag.PunetoriId = new SelectList(new List<string>(), "Value", "Text");
            ViewBag.GradaId = await gradaRepository.GradaSelectList(null ,false, false);
            ViewBag.BankaId = await bankRepository.BankaSelectList(null, false, false);

            return View();
        }

        public async Task<IActionResult> GenerateReport(RaportViewModel model)
        {
            if(model.RaportiId == (int)Raportet.PagatTabelare)
            {
                if(User.IsInRole("User"))
                {
                    var punetori = await punetoriRepository.GetPunetoriByUserId(user.UserId);
                    model.PunetoriId = punetori.Id;
                }

                var pagat = await raportRepository.GetAllPagat(model.PunetoriId, model.KompaniaId, model.Viti, model.Muaji, model.BankaId, model.GradaId);

                Models.SessionExtensions.Set(session, "PagatTabelare", pagat);
                ViewBag.RaportiId = model.RaportiId;
                return View(pagat);
            }
            else
            {

            }

            return null;
        }

        public async Task<ActionResult> Print(int raportid)
        {
             
            return null;
        }

        // GET: RaportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RaportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RaportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: RaportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RaportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: RaportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RaportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
