using Graduation_Project.Application.DTOs.OurServiceDTOs;
using Graduation_Project.Application.Services;
using Graduation_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Interfaces.IServices
{
    public interface IOurService 
    {
        Task<IEnumerable<ServiceEntity?>> GetAllAsync();
        Task<DisplayServices> GetById(int id);

        Task<ServiceEntity?> GetByIdAsync(int id);
        Task AddAsync(OurserviceDTO entity);
         //Task<bool> UpdateAsync(int id, OurService dto);
        Task DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, OurserviceDTO serviceDto);
    }
}
