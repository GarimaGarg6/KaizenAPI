using FABQC.Core.DataAccess;
using FABQC.Core.Repository;
using FABQC.Core.Repository.Contracts;
using FABQC.Domain.Entities;
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
    public class FactoryController : BaseController
    {
        IFABRepository repository = new FABSqlRepository(new FABSqlContext());

        [HttpGet]
        public HttpResponseMessage Factory(Guid factoryId)
        {
            Factory factory = null;
            var result = new HttpResponseMessage();
            if (factoryId != null)
            {
                factory = repository.FindOne<Factory>(x => x.FactoryId == factoryId);
            }
            else
            {
                factory = new Factory();
            }
            var data = JsonConvert.SerializeObject(factory);
            result.StatusCode = HttpStatusCode.OK;
            result.Content = new StringContent(data);
            return result;
        }

        [HttpPost]
        public HttpResponseMessage SaveFactory(Factory factory)
        {
            var result = new HttpResponseMessage();
            if (ModelState.IsValid)
            {
                    factory.IsActive = true;
                    factory.CreatedDate = DateTime.Now;
                    repository.Add<Factory>(factory);
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
        public HttpResponseMessage DeleteFactory(Guid factoryId)
        {
            var result = new HttpResponseMessage();
            if (factoryId != null)
            {
                Factory factory = repository.FindOne<Factory>(x => x.FactoryId == factoryId);
                factory.IsActive = false;
                factory.UpdatedDate = DateTime.Now;

                List<Factory> factories = repository.GetAll<Factory>().Where(x => x.IsActive == true).ToList();
                repository.Update<Factory>(factory);
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
                    successMessage = "Factory Not Found"
                };
                var data = JsonConvert.SerializeObject(msg);

                result.StatusCode = HttpStatusCode.NotFound;
                result.Content = new StringContent(data);
                return result;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateFactory(Factory factory)
        {
            var result = new HttpResponseMessage();
            ModelState.Remove("CreatedBy");
            if (ModelState.IsValid)
            {
                if (factory != null && factory.FactoryId != null)
                {
                    Factory editedFactory = repository.FindOne<Factory>(x => x.FactoryId == factory.FactoryId);

                    if (editedFactory != null)
                    {
                        editedFactory.FactoryName = factory.FactoryName;
                        editedFactory.FactoryCode = factory.FactoryCode;
                        editedFactory.Address = factory.Address;
                        editedFactory.Contact = factory.Contact;
                        editedFactory.CompanyId = factory.CompanyId;
                        editedFactory.UpdatedBy = factory.UpdatedBy;
                        editedFactory.UpdatedDate = DateTime.Now;
                    }
                    repository.Update<Factory>(editedFactory);
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
                        successMessage = "Factory Not Found"
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
        public HttpResponseMessage Factories()
        {
            var factories = new List<Factory>();
            factories = repository.GetAll<Factory>()
                .Where(x => x.IsActive == true)
                .ToList();
            var data = JsonConvert.SerializeObject(factories);
            var result = new HttpResponseMessage();
            result.Content = new StringContent(data);
            return result;
        }
    }
}