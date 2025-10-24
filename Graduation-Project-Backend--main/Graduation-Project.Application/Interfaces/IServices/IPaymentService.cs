using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Application.DTOs.PaymentDTOs;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Interfaces.IServices
{
    public interface IPaymentService
    {
        public Task<int> CreatePayment(CreatePaymentDTO paymentDTO);


    }
}
