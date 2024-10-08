using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WorksManagement.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Project Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Project Code")]
        public string Code { get; set; }

        [Required]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [DisplayName("Project Status")]
        public ProjectStatus Status { get; set; }
    }

    public enum ProjectStatus
    {
        New,
        Ongoing,
        Postponed,
        Finished,
        Cancelled
    }
}
