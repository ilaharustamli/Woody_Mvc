namespace Woody_Mvc.ViewModels.TeamMember
{
    public record CreateTeamMemberVm
    {
        public string Name { get; set; }
        public int positionId { get; set; }

        public IFormFile File { get; set; }
    }
}
