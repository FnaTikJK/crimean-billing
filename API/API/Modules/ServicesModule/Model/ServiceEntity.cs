using System.ComponentModel.DataAnnotations;
using API.DAL;

namespace API.Modules.ServicesModule.Model;

public class ServiceEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public ServiceTemplateEntity Template { get; set; }
    public float Price { get; set; }
    public float? Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}