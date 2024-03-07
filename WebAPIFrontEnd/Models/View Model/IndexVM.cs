namespace WebAPIFrontEnd.Models.View_Model
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> nationalParkList { get; set; }
        public IEnumerable<Trails> trailList { get; set; }
    }
}
