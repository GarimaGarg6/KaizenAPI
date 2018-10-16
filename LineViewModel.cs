using FABQC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FABQC.Web.API.Models
{
    public class LineViewModel
    {
        public Line Line { get; set; }

        public List<SelectListItem> factoryListItems { get; set; }

        public List<Factory> factories { get; set; }

        public Guid SelectedFactoryId { get; set; }
    }
}