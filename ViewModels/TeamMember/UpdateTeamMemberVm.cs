namespace Woody_Mvc.ViewModels.TeamMember
{
    public record UpdateTeamMemberVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int positionId { get; set; }

        public IFormFile? File { get; set; }
    }
}
