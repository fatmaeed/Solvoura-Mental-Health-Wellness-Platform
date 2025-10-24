using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Domain.Enums;

namespace Graduation_Project.Application.DTOs.PaymentDTOs
{
    public class CreatePaymentDTO
    {
        public decimal Amount { get; set; }

        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; }

        public string TransactionId { get; set; }

    }
}
