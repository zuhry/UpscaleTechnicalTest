using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpscaleTechnicalTest.Models;

public class TodoModel
{
    [Key, Column(name: "Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [StringLength(150)]
    public string Title { get; set; }
    [Required]
    [StringLength(750)]
    public string Description { get; set; }
    [Required]
    [StringLength(50)]
    public string Scope { get; set; }
    [Required]
    public int Priority { get; set; }
    [Required]
    public DateTime Deadline { get; set; }
    [Display(Name = "Is Completed?")]
    public bool? IsCompleted { get; set; }
    [StringLength(250)]
    public string EmailNotification { get; set; } = string.Empty;
    public bool IsNotificationSend { get; set; } = false;
    [Display(Name = "Created Date")]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm}")]
    public DateTime CreatedDate { get; set; }
    [Display(Name = "Updated Date")]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm}")]
    public DateTime? UpdatedDate { get; set; }
}