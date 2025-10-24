using AutoMapper;
using Graduation_Project.Application.DTOs.ClientDTOs;
using Graduation_Project.Application.DTOs.OurServiceDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Graduation_Project.Application.Services
{
    public class OurService :  IOurService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OurService(IUnitOfWork unitOfWork,IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DisplayServices> GetById(int id)
        {
            var service = await Task.Run(() => _unitOfWork.ServiceRepo.Get(id));

            if (service == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }

            var serviceDto = _mapper.Map<DisplayServices>(service);
            return serviceDto;
        }

        public async Task AddAsync(OurserviceDTO serviceDto)
        {
            if (serviceDto == null)
            {
                throw new ArgumentNullException(nameof(serviceDto), "Service data cannot be null");
            }
            var serviceEntity = _mapper.Map<ServiceEntity>(serviceDto);
            _unitOfWork.ServiceRepo.Add(serviceEntity);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var service = _unitOfWork.ServiceRepo.Get(id);
            if (service == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }
            service.IsDeleted = true;
            _unitOfWork.ServiceRepo.Update(service);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServiceEntity?>> GetAllAsync()
        {
            return _unitOfWork.ServiceRepo.GetAll();
        }

        public async Task<ServiceEntity?> GetByIdAsync(int id)
        {
            return _unitOfWork.ServiceRepo.Get(id);
        }

        public Task<bool> UpdateAsync(int id, OurserviceDTO serviceDto)
        {
            var existingService = _unitOfWork.ServiceRepo.Get(id);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }
            _mapper.Map(serviceDto, existingService);
            if (serviceDto.Icon != null)
            {
                existingService.Icon = serviceDto.Icon;
            }
            _unitOfWork.ServiceRepo.Update(existingService);
            return _unitOfWork.SaveChangesAsync().ContinueWith(t => t.IsCompletedSuccessfully);
        }

      
    }
}
