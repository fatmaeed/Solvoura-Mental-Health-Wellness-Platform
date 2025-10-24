using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ServiceProviderDTOs
{
    public class DisplayServiceProviderDTO
    {
        public int Id { get; set; }
        public string Specialization { get; set; }
        public bool isApproved { get; set; }
        public string UserImagePath { get; set; }
        public  string NationalImagePath { get; set; }
        public   List<CertificateDTO> Certificates { get; set; }
        public string UserAndNationalImagePath { get; set; }
        public  string Description { get; set; }
        public int NumberOfExp { get; set; } 
        public string? Experience { get; set; }
        public decimal PricePerHour { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string? ClinicLocation { get; set; }
        //------
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string ExaminationType { get; set; }
      
        public string PhoneNumber  { get;  set; }

    }
    public class CertificateDTO
    {
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
