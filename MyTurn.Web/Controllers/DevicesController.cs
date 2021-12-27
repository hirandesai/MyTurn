using Microsoft.AspNet.SignalR;
using MyTurn.Web.Hubs;
using MyTurn.Web.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Timers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace MyTurn.Web.Controllers
{
    public class DevicesController : ApiController
    {
        private DevTimer timer = null;
        private const int timerMiliSeconds = 2000;

        
        public IEnumerable<Device> Get(int? Id = 0)
        {
            IEnumerable<Device> devices = new List<Device>();
            if (Id.HasValue && Id.Value != 0)
            {
                try
                {
                    var context = GlobalHost.ConnectionManager.GetHubContext<MyTurnHub>();
                    timer = TimerContainer._Timers.FirstOrDefault(s => s.Identifier.Equals(Id));

                    if (timer != null)
                    {
                        timer.Stop();
                        if (!timer.LastStatus)
                        {
                            timer.LastStatus = true;
                            UpdateDeviceState(Id.Value, true);
                            context.Clients.All.triggerToilet(Id.Value, true);
                        }
                        timer.Start();
                    }
                    else
                    {
                        using (var db = new myturnEntities())
                        {
                            var device = db.Devices
                                        .Include("Toilet")
                                        .FirstOrDefault(s => s.Id.Equals(Id.Value));
                            if (device != null)
                            {
                                if (device.Toilet != null)
                                {
                                    device.Toilet.IsInUse = true;
                                    db.SaveChanges();
                                    context.Clients.All.triggerToilet(device.ToiletId, true);
                                    #region Initialize Timer
                                    timer = new DevTimer();
                                    timer.Identifier = Id.Value;
                                    timer.AutoReset = true;
                                    timer.Enabled = true;
                                    timer.Interval = timerMiliSeconds;
                                    timer.Elapsed += timer_Elapsed;
                                    timer.LastStatus = true;
                                    timer.Start();
                                    TimerContainer._Timers.Add(timer);
                                    #endregion
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {

                try
                {
                    using (var db = new myturnEntities())
                    {
                        devices = db.Devices
                                    .Where(s => s.IsActive).AsEnumerable().Select(s => new Device()
                                    {
                                        Id = s.Id,
                                        Identifier = s.Identifier,
                                        IsActive = s.IsActive
                                    }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return devices;

        }

        //public ActionResult Get(int Id)
        //{
        //    try
        //    {
        //        using (var db = new myturnEntities())
        //        {
        //            var device = db.Devices.FirstOrDefault(s => s.Id.Equals(Id) && s.IsActive);
        //            if (device != null)
        //            {
        //                var result = new JsonResult();
        //                result.Data = device;
        //                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return new HttpNotFoundResult(string.Format("Device not found for Id {0}", Id));
        //}

        public ActionResult Post([FromBody]Device NewDevice)
        {
            try
            {
                using (var db = new myturnEntities())
                {
                    if (db.Devices.Any(s => s.Identifier.Equals(NewDevice.Identifier)))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Device exists with same Identifier");
                    }
                    NewDevice.Id = 0;
                    NewDevice.IsActive = true;
                    db.Devices.Add(NewDevice);
                    db.SaveChanges();
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Device registered successfully");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Some error occured on server");
        }

        public ActionResult Put(int Id, [FromBody]Device NewDevice)
        {
            try
            {
                using (var db = new myturnEntities())
                {
                    var ExistingDevice = db.Devices.FirstOrDefault(s => s.Id.Equals(Id));
                    if (ExistingDevice != null)
                    {
                        ExistingDevice.Identifier = NewDevice.Identifier;
                        ExistingDevice.IsActive = NewDevice.IsActive;
                        db.SaveChanges();
                        return new HttpStatusCodeResult(HttpStatusCode.OK, "Device updated successfully");
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No such device exists.");
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
                    var device = db.Devices.FirstOrDefault(s => s.Id.Equals(Id));
                    if (device != null)
                    {
                        device.IsActive = false;
                        db.SaveChanges();
                        return new HttpStatusCodeResult(HttpStatusCode.OK, "Device deleted successfully");
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No such device exists.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Some error occured on server");
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var _timer = sender as DevTimer;
            var _context = GlobalHost.ConnectionManager.GetHubContext<MyTurnHub>();
            if (_timer.LastStatus)
            {
                _timer.LastStatus = false;
                UpdateDeviceState(_timer.Identifier, false);
                _context.Clients.All.triggerToilet(_timer.Identifier, false);
            }
        }

        private void UpdateDeviceState(int id, bool isInUse)
        {
            using (var dbContext = new myturnEntities())
            {
                var deviceToOff = dbContext.Devices
                    .Include("Toilet")
                    .FirstOrDefault(s => s.Id.Equals(id));
                deviceToOff.Toilet.IsInUse = isInUse;
                dbContext.SaveChanges();
            }
        }
    }

    public class DevTimer : Timer
    {
        public bool LastStatus { get; set; }
        public int Identifier { get; set; }
    }

    public class TimerContainer
    {
        public static List<DevTimer> _Timers = new List<DevTimer>();
    }
}