﻿using Client.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class TrailVM
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }

        public Trail Trail { get; set; }
    }
}
