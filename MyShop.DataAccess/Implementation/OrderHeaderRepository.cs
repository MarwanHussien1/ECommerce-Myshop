using Myshop.DataAccess.data;
using Myshop.Entities.Models;
using Myshop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.DataAccess.Implementation
{
	public class OrderHeaderRepository: GenericRepository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

		public void Update(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Update(orderHeader);
		}

		public void UpdateOrderStatus(int id, string? OrderStatus, string? PaymentStatus)
		{
			var orderFromDB = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
			if (orderFromDB != null)
			{
				orderFromDB.OrderStatus = OrderStatus;
				orderFromDB.OrderDate = DateTime.Now;
				if(PaymentStatus != null)
				{
					orderFromDB.PaymentStatus = PaymentStatus;

				}
			}
		}
	}
}
