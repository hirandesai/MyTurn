using MyTurn.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace MyTurn.Web.Controllers
{
    public class ToiletsController : ApiController
    {
        
        public IEnumerable<Toilet> Get()
        {           
            IEnumerable<Toilet> toilets = new List<Toilet>();
            try
            {
                using (var db = new myturnEntities())
                {
                    toilets = db.Toilets
                                .Include("Group")
                                .Where(s => s.IsActive).AsEnumerable().Select(s => new Toilet()
                                {
                                    Id = s.Id,
                                    Identifier = s.Identifier,
                                    IsActive = s.IsActive,
                                    IsInUse = s.IsInUse,
                                    Group = new Group()
                                    {
                                        Id = s.Group.Id,
                                        Name = s.Group.Name,
                                        IsActive = s.Group.IsActive
                                    }
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return toilets;            
        }        
    }    
}