﻿using FranchiseMenu.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FranchiseMenu.ENTITY.Dtos.CategoryDtos
{
    public class CategoryAddDto : IDto
    {
        public string Token { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public bool CategoryStatus { get; set; }
    }
}