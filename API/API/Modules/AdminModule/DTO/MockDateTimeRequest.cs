using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Modules.AdminModule.DTO;

public class MockDateTimeRequest
{
    [RegularExpression(@"\d{4}-\d{2}-\d{2}")]
    [DefaultValue("2024-12-31")]
    public string Date { get; set; }
}