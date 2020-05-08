﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Requests.Dto
{
    public class CreateOrEditRequestDto : EntityDto<int?>
    {
        public int TourId { get; set; }

        public string Description { get; set; }
    }
}