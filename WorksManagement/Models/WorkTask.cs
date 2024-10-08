using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WorksManagement.Models
{
    public class WorkTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Task Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Task Code")]
        public string Code { get; set; }

        // Removing ProjectId and using ProjectName instead
        [Required]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        [Required]
        [DisplayName("Task Status")]
        public TaskStatus Status { get; set; }
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Done
    }
}
