namespace Agri_EnergyConnect.Models.ViewModels
{
    public class ProjectCommentViewModel
    {
        public ProjectCollaboration Project { get; set; }
        public List<ProjectComment> Comments { get; set; }
        public ProjectComment NewComment { get; set; }
    }
}
