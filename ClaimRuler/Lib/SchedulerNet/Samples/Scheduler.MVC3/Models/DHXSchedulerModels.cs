
namespace SchedulerTest.Models
{
    using System.ComponentModel.DataAnnotations;
    [MetadataType(typeof(ValidEventMetadata))]
    public partial class ValidEvent
    {
    }

   
    


    public class ValidEventMetadata
    {
        [Required]
        public string text { get; set; }

        [Required]
        [RegularExpression(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$", ErrorMessage = "Invalid email")]
        public string email { get; set; }

            }

}
