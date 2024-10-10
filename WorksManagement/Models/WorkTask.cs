using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("Project")]
        [DisplayName("Project")]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

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
