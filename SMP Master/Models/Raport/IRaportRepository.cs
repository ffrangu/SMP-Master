using SMP.ViewModels.Paga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Raport
{
    public interface IRaportRepository
    {
        Task<List<AllPagat>> GetAllPagat(int? PunetoriId, int? KompaniaId, int? Viti, int? Muaji, int? BankaId, int? GradaId);
    }
}
