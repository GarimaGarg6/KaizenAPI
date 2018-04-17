using KaizenAPI.App_Start;
using KaizenAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Linq;
using System.Web.Mvc;

namespace KaizenAPI.Controllers
{
    public class CMClassController : Controller
    {
        MongoContext _dbContext;

        public CMClassController()
        {
            _dbContext = new MongoContext();
        }

        // GET: CMClass
        public ActionResult Index()
        {
            var classDetails = _dbContext._database.GetCollection<CMClass>("CMClass").FindAll().ToList();
            return View(classDetails);
        }

        // GET: CMClass/Details/5
        public ActionResult Details(string id)
        {
            var classId= Query<CMClass>.EQ(p => p.Id ,new ObjectId(id));
            var classDetail = _dbContext._database.GetCollection<CMClass>("CMClass").FindOne(classId);
            return View(classDetail);
        }

        // GET: CMClass/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CMClass/Create
        [HttpPost]
        public ActionResult Create(CMClass cMClass)
        {
            var document = _dbContext._database.GetCollection<BsonDocument>("CMClass");

            var query = Query.And(Query.EQ("Class_Id", cMClass.Class_Id), Query.EQ("Class", cMClass.Class));

            var count = document.FindAs<CMClass>(query).Count();

            if (count == 0)
            {
                var result = document.Insert(cMClass);
            }
            else
            {
                TempData["Message"] = "ClassName Already Exists";
                return View("Create", cMClass);
            }

            return RedirectToAction("Index");
        }

        // GET: CMClass/Edit/5
        public ActionResult Edit(string id)
        {
            var document = _dbContext._database.GetCollection<CMClass>("CMClass");
            var classDetailsCount = document.FindAs<CMClass>(Query.EQ("_id", new ObjectId(id))).Count();

            if (classDetailsCount > 0)
            {
                var classObjectId = Query<CMClass>.EQ(p => p.Id, new ObjectId(id));
                var classDetail = _dbContext._database.GetCollection<CMClass>("CMClass").FindOne(classObjectId);
                return View(classDetail);
            }
            return RedirectToAction("Index");

        }

        // POST: CMClass/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, CMClass cMClass)
        {
            try
            {
                cMClass.Id = new ObjectId(id);
                var classObjectId = Query<CMClass>.EQ(p => p.Id, new ObjectId(id));
                var collection = _dbContext._database.GetCollection<CMClass>("CMClass");

                var result = collection.Update(classObjectId, Update.Replace(cMClass), UpdateFlags.None);
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: CMClass/Delete/5
        public ActionResult Delete(string id)
        {
            var classObjectId = Query<CMClass>.EQ(p => p.Id, new ObjectId(id));
            var classdetails = _dbContext._database.GetCollection<CMClass>("CMClass").FindOne(classObjectId);
            return View(classdetails);
        }

        // POST: CMClass/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, CMClass cMClass)
        {
            try
            {
                // TODO: Add delete logic here
                var classObjectId = Query<CMClass>.EQ(p => p.Id, new ObjectId(id));
                var collection = _dbContext._database.GetCollection<CMClass>("CMClass");
                var result = collection.Remove(classObjectId, RemoveFlags.Single);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
