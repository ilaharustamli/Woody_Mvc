using Woody_Mvc.Models.Base;

namespace Woody_Mvc.Models
{
    public class TeamMember :BaseEntity
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public int positionId { get; set; }
        public Position Position { get; set; }
    }
}
