using WebAPI.Models;

namespace WebAPI.Data.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trails> GetTrails();
        Trails GetTrail(int TrailsId);
        ICollection<Trails> GetTrailsInNationalPark(int NationalParkId);
        bool TrailsExists(int TrailsId);
        bool TrailsExists(string TrailsName);
        bool CreateTrails(Trails Trails);
        bool UpdateTrails(Trails Trails);
        bool DeleteTrails(Trails Trails);
        bool Save();
    }
}
