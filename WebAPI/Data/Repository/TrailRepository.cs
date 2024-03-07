using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Repository.IRepository;
using WebAPI.Models;

namespace WebAPI.Data.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _context;

        public TrailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateTrails(Trails Trails)
        {
            _context.Trails.Add(Trails);
            return Save();
        }

        public bool DeleteTrails(Trails Trails)
        {
            _context.Trails.Remove(Trails);
            return Save();
        }

        public Trails GetTrail(int TrailsId)
        {
            return _context.Trails.Include(t => t.NationalPark).FirstOrDefault(t => t.Id == TrailsId);
        }

        public ICollection<Trails> GetTrails()
        {
            return _context.Trails.Include(t=>t.NationalPark).ToList();
        }


        public ICollection<Trails> GetTrailsInNationalPark(int NationalParkId)
        {
            return _context.Trails.Include(t=>t.NationalPark).Where(t=>t.NationalParkId== NationalParkId).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges()==1?true:false;
        }

        public bool TrailsExists(int TrailsId)
        {
            return _context.Trails.Any(t=>t.Id==TrailsId);
        }

        public bool TrailsExists(string TrailsName)
        {
            return _context.Trails.Any(t=>t.Name==TrailsName);
        }

        public bool UpdateTrails(Trails Trails)
        {
            _context.Trails.Update(Trails);
            return Save();
        }
    }
}
