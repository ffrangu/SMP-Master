using Microsoft.EntityFrameworkCore;
using SMP.Data;
using SMP.ViewModels.Paga;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Raport
{
    public class RaportRepository : IRaportRepository
    {
        protected readonly ApplicationDbContext context;

        public RaportRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<List<AllPagat>> GetAllPagat(int? PunetoriId, int? KompaniaId, int? Viti, int? Muaji, int? BankaId, int? GradaId)
        {
            List<Data.Paga> pagat = new List<Data.Paga>();
            if(PunetoriId.HasValue)
            {
                pagat = await context.Paga.Where(q => q.PunetoriId == PunetoriId.Value)
                                          .Include(q => q.Punetori)
                                          .Include(q => q.Punetori.Pozita)
                                          .Include(q => q.Grada)
                                          .Include(q => q.Punetori.Banka)
                                          .Include(q => q.Kompania).OrderByDescending(q => q.Id).ToListAsync();
            }
            else
            {
                if(KompaniaId.Value != 0)
                {
                    pagat = await context.Paga.Where(q => q.KompaniaId == KompaniaId)
                                              .Include(q => q.Punetori)
                                              .Include(q => q.Punetori.Pozita)
                                              .Include(q => q.Grada)
                                              .Include(q => q.Punetori.Banka)
                                              .Include(q => q.Kompania).OrderByDescending(q => q.Id).ToListAsync();
                }
                else
                {
                    pagat = await context.Paga
                          .Include(q => q.Punetori)
                          .Include(q => q.Punetori.Pozita)
                          .Include(q => q.Grada)
                          .Include(q => q.Kompania).OrderByDescending(q => q.Id).ToListAsync();
                }
            }

            if(Viti.HasValue)
            {
                pagat = pagat.Where(q => q.Viti == Viti.Value).ToList();
            }

            if (Muaji.HasValue)
            {
                pagat = pagat.Where(q => q.Muaji == Muaji.Value).ToList();
            }

            if (GradaId.HasValue)
            {
                pagat = pagat.Where(q => q.GradaId == GradaId.Value).ToList();
            }

            if (BankaId.HasValue)
            {
                pagat = pagat.Where(q => q.Punetori.Banka.Id == BankaId.Value).ToList();
            }

            var allPagat = (from p in pagat
                            select new AllPagat
                            {
                                Id = p.Id,
                                Punetori = p.Punetori.Emri + " " + p.Punetori.Mbiemri,
                                Kompania = p.Kompania.Emri,
                                Pozita = p.Punetori.Pozita.Emri,
                                Grada = p.Grada.Emri,
                                Viti = p.Viti,
                                Muaji = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(p.Muaji),
                                MuajiInt = p.Muaji,
                                Bruto = p.Bruto,
                                Kontributi = p.KontributiPunetori,
                                PagaPaTatimuar = p.PagaTatim,
                                Tatimi = p.Tatimi,
                                PagaNeto = p.PagaNeto,
                                Bonuse = p.Bonuse,
                                BonuseNeto = p.BonuseNeto,
                                PagaFinale = p.PagaFinale,
                                Banka = p.Punetori.Banka.Emri
                            }).ToList();

            return allPagat;
        }
    }
}
