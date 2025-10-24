using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Graduation_Project.Application.DTOs.PaymentDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<int> CreatePayment(CreatePaymentDTO paymentDTO)
        {
            var payment = _mapper.Map<PaymentEntity>(paymentDTO);
            var CreatedPayment = _unitOfWork.PaymentRepo.Add(payment);
            await _unitOfWork.SaveChangesAsync();
            return CreatedPayment.Id;
        }
    }
}
