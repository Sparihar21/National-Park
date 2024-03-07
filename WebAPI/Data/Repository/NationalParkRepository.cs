using WebAPI.Data.Repository.IRepository;
using WebAPI.Models;

namespace WebAPI.Data.Repository
{
    public class NationalParkRepository:INationalParkRepository
    {
        private readonly ApplicationDbContext _context;
        public NationalParkRepository(ApplicationDbContext context)
        {
            _context= context;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _context.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _context.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int NationalParkId)
        {
            return _context.NationalParks.Find(NationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _context.NationalParks.ToList();
        }

        public bool NationalParkExists(int NationalParkId)
        {
            return _context.NationalParks.Any(np => np.Id == NationalParkId);
        }

        public bool NationalParkExists(string NationalParkName)
        {
            return _context.NationalParks.Any(np=>np.Name== NationalParkName);
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _context.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
