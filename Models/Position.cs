using Woody_Mvc.Models.Base;

namespace Woody_Mvc.Models
{
    public class Position :BaseEntity
    {
        public string Name { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
    }
}
