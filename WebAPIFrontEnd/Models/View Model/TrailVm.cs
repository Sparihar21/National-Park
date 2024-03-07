using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAPIFrontEnd.Models.View_Model
{
    public class TrailVm
    {
        public IEnumerable<SelectListItem> nationalParkList { get; set; }
        public Trails Trails { get; set; }
    }
}
