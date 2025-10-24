using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ClientDTOs
{
    public class DisplayClientDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAnon { get; set; }
        public bool IsVerified { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }

        public DateOnly BirthDate { get; set; }
        public string? UserImagePath { get; set; }

        [Phone, RegularExpression("^01[0-2,5]{1}[0-9]{8}$")]
        public string? AlternativePhoneNumber { get; set; }

        public string? HistoryIllness { get; set; }

    }
}