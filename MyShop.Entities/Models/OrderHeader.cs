using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Models
{
	public class OrderHeader
	{
		public int Id { get; set; }

		public string ApplicationUserId { get; set; }
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }

		public DateTime? OrderDate { get; set; } = DateTime.Now;

		public DateTime ShippingDate {  get; set; } 

		public decimal TotalPrice { get; set; }

		public string? OrderStatus { get; set; }

		public string? PaymentStatus { get; set; }

		public string? TrackingNumber { get; set; }

		public string? Carrier {  get; set; }

		public DateTime? PaymentDate { get; set; }

		// stripe
		public string? SessionId { get; set; }
		public string? PaymentIntentId { get; set; }

		//Data of user
		public string Name { get; set; }
		public string Address { get; set; }	

		public string AddressCity { get; set; }

		public string? AddressRegion { get; set; }

		public string PhoneNumber	{ get; set; }
	}
}
