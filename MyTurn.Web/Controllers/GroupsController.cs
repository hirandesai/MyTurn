using MyTurn.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace MyTurn.Web.Controllers
{    
    public class GroupsController : ApiController
    {
        public IEnumerable<Group> Get()
        {
            IEnumerable<Group> groups = new List<Group>();
            try
            {
                using (var db = new myturnEntities())
                {
                    groups = db.Groups.Where(s => s.IsActive).AsEnumerable().Select(s => new Group()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsActive = s.IsActive
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return groups;
        }

        public ActionResult Get(int Id)
        {
            try
            {
                using (var db = new myturnEntities())
                {
                    var group = db.Groups.FirstOrDefault(s => s.Id.Equals(Id) && s.IsActive);
                    if (group != null)
                    {
                        var result = new JsonResult();
                        result.Data = group;
                        result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new HttpNotFoundResult(string.Format("Group not found for Id {0}", Id));
        }

        public ActionResult Post([FromBody]Group NewGroup)
        {
            try
            {
                using (var db = new myturnEntities())
                {
                    if (db.Groups.Any(s => s.Name.Equals(NewGroup.Name)))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Group exists with same name");
                    }
                    NewGroup.Id = 0;
                    NewGroup.IsActive = true;
                    db.Groups.Add(NewGroup);
                    db.SaveChanges();
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Group created successfully");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Some error occured on server");
        }
                
        public ActionResult Put(int Id, [FromBody]Group NewGroup)
        {
            try
            {
                using (var db = new myturnEntities())
                {
                    var ExistingGroup = db.Groups.FirstOrDefault(s => s.Id.Equals(Id));
                    if (ExistingGroup != null)
                    {
                        ExistingGroup.Name = NewGroup.Name;
                        ExistingGroup.IsActive = NewGroup.IsActive;
                        db.SaveChanges();
                        return new HttpStatusCodeResult(HttpStatusCode.OK, "Group updated successfully");
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No such group exists.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Some error occured on server");
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                using (var db = new myturnEntities())
                {
                    var ExistingGroup = db.Groups.FirstOrDefault(s => s.Id.Equals(Id));
                    if (ExistingGroup != null)
                    {
                        ExistingGroup.IsActive = false;
                        db.SaveChanges();
                        return new HttpStatusCodeResult(HttpStatusCode.OK, "Group deleted successfully");
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No such group exists.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Some error occured on server");
        }

    }
}
