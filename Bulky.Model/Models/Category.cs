using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Model.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[DisplayName("Category Name")]
		[MaxLength(30)]
		public string Name { get; set; }
        [DisplayName("Display Order")]
		[Range(1,100, ErrorMessage ="Display order must be 1_100")]
        public int DisplayOrder { get; set; }
	}
}
