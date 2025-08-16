using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Models
{
	public class OrderDetail
	{
		public int Id { get; set; }
		public int OrderId { get; set; }

		public int orderHeaderId { get; set; }

		[ValidateNever]
		public OrderHeader orderHeader { get; set; }

		public int ProductId { get; set; }

		[ValidateNever]
		public Product Product { get; set; }

		public int count { get; set; }

		public decimal price { get; set; }
	}
}
