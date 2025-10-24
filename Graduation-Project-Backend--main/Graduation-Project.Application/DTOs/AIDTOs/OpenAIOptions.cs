﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.AIDTOs
{
    public class OpenAIOptions
    {
        public string ApiKey { get; set; }
        public string Model { get; set; } = "gpt-4o";
    }
}
