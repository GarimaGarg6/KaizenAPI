using FABQC.Core.DataAccess;
using FABQC.Core.Repository;
using FABQC.Core.Repository.Contracts;
using FABQC.Domain.Entities;
using FABQC.Web.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;


namespace FABQC.Web.API.Controllers
{
    public class LineController : BaseController
    {
        IFABRepository repository = new FABSqlRepository(new FABSqlContext());

        [HttpGet]
        public HttpResponseMessage Line(Guid lineId)
        {
            LineViewModel vm = new LineViewModel();
            var result = new HttpResponseMessage();
            vm.factoryListItems = GetAllFactories();

            if (lineId != null)
            {
               Line line = repository.FindOne<Line>(x => x.LineId == lineId);
                vm.SelectedFactoryId = line.FactoryId;
                vm.Line = line;
            }
            else
            {
                vm.Line = new Line();
            }
            var data = JsonConvert.SerializeObject(vm.Line);
            result.StatusCode = HttpStatusCode.OK;
            result.Content = new StringContent(data);
            return result;
        }

        [HttpPost]
        public HttpResponseMessage SaveLine(LineViewModel postedLine)
        {
            var result = new HttpResponseMessage();
            if (ModelState.IsValid)
            {
                    postedLine.Line.FactoryId = postedLine.SelectedFactoryId;
                    postedLine.Line.IsActive = true;
                    postedLine.Line.CreatedDate = DateTime.Now;
                    repository.Add<Line>(postedLine.Line);
                    repository.UnitOfWork.SaveChanges();

                    var msg = new ApiMessage()
                    {
                        successMessage = "Saved Successfully"
                    };

                    var data = JsonConvert.SerializeObject(msg);
                    result.StatusCode = HttpStatusCode.OK;
                    result.Content = new StringContent(data);
                    return result;
              
            }
            else
            {
                var msg = new ApiMessage()
                {
                    successMessage = "Please review all fields."
                };

                var data = JsonConvert.SerializeObject(msg);
                result.StatusCode = HttpStatusCode.NotFound;
                result.Content = new StringContent(data);
                return result;
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteLine(Guid lineId)
        {
            var result = new HttpResponseMessage();
            if (lineId !=null)
            {
                Line line = repository.FindOne<Line>(x => x.LineId == lineId);
                line.IsActive = false;
                line.UpdatedDate = DateTime.Now;

                List<Line> lines = repository.GetAll<Line>().Where(x => x.IsActive == true).ToList();
                repository.Update<Line>(line);
                repository.UnitOfWork.SaveChanges();

                var msg = new ApiMessage()
                {
                    successMessage = "Deleted Successfully"
                };
                var data = JsonConvert.SerializeObject(msg);

                result.StatusCode = HttpStatusCode.OK;
                result.Content = new StringContent(data);
                return result;
            }
            else
            {
                var msg = new ApiMessage()
                {
                    successMessage = "Line Not Found"
                };
                var data = JsonConvert.SerializeObject(msg);

                result.StatusCode = HttpStatusCode.NotFound;
                result.Content = new StringContent(data);
                return result;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateLine(LineViewModel updatedVM)
        {
            var result = new HttpResponseMessage();
            ModelState.Remove("CreatedBy");
            if (ModelState.IsValid)
            {
                if (updatedVM != null && updatedVM.Line != null)
                {
                    Line editedLine = repository.FindOne<Line>(x => x.LineId == updatedVM.Line.LineId);

                    if (editedLine != null)
                    {
                        editedLine.LineName = updatedVM.Line.LineName;
                        editedLine.LineDesc = updatedVM.Line.LineDesc;
                        editedLine.FactoryId = updatedVM.SelectedFactoryId;

                        editedLine.UpdatedBy = updatedVM.Line.UpdatedBy;
                        editedLine.UpdatedDate = DateTime.Now;
                    }
                    repository.Update<Line>(editedLine);
                    repository.UnitOfWork.SaveChanges();
                    var msg = new ApiMessage()
                    {
                        successMessage = "Updated Successfully"
                    };

                    var data = JsonConvert.SerializeObject(msg);

                    result.StatusCode = HttpStatusCode.OK;
                    result.Content = new StringContent(data);
                    return result;
                }
                else
                {
                    var msg = new ApiMessage()
                    {
                        successMessage = "Line Not Found"
                    };

                    var data = JsonConvert.SerializeObject(msg);

                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Content = new StringContent(data);
                    return result;
                }
            }
            else
            {
                var msg = new ApiMessage()
                {
                    successMessage = "Please review all required fields"
                };

                var data = JsonConvert.SerializeObject(msg);

                result.StatusCode = HttpStatusCode.NotFound;
                result.Content = new StringContent(data);
                return result;
            }

        }

        [HttpGet]
        public HttpResponseMessage Lines()
        {
            var lines = new List<Line>();
            lines = repository.GetAll<Line>()
                .Where(x => x.IsActive == true)
                .ToList();
            var data = JsonConvert.SerializeObject(lines);
            var result = new HttpResponseMessage();
            result.Content = new StringContent(data);
            return result;
        }

        [HttpGet]
        public HttpResponseMessage LinesforFactory(Guid factoryId)
        {
            var lines = new List<Line>();
            lines = repository.GetAll<Line>()
                .Where(x => x.IsActive == true && x.FactoryId == factoryId)
                .ToList();
            var data = JsonConvert.SerializeObject(lines);
            var result = new HttpResponseMessage();
            result.Content = new StringContent(data);
            return result;
        }

        public List<System.Web.Mvc.SelectListItem> GetAllFactories()
        {
            List<System.Web.Mvc.SelectListItem> factoryItems = new List<System.Web.Mvc.SelectListItem>();
            List<Factory> factories = this.Repository.GetAll<Factory>().Where(x => x.IsActive == true).ToList();

            factoryItems.Add(new System.Web.Mvc.SelectListItem { Text = "--Select Factory--", Value = "0" });
            foreach (var item in factories)
            {
                factoryItems.Add(new System.Web.Mvc.SelectListItem { Text = item.FactoryName, Value = item.FactoryId.ToString() });
            }
            return factoryItems;

        }
    }
}