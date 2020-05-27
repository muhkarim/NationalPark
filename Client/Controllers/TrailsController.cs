using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;
using Client.Repository.IRepository;
using Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Client.Controllers
{
    [Authorize]
    public class TrailsController : Controller
    {

        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;


        public TrailsController( INationalParkRepository nationalParkRepository, ITrailRepository trailRepository)
        {
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
        }


        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            // create object list nationalpark
            IEnumerable<NationalPark> npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken"));
            TrailVM objVM = new TrailVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail() // add object reference to acces Id
            };

            

            if(id == null)
            {
                // this will be true for insert/create
                return View(objVM);
            }

            objVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if(objVM.Trail == null)
            {
                return NotFound();
            }

            return View(objVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailVM obj)
        {
            if (ModelState.IsValid)
            {
             
                if (obj.Trail.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, obj.Trail, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath + obj.Trail.Id, obj.Trail, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // create object list nationalpark
                IEnumerable<NationalPark> npList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken"));
                TrailVM objVM = new TrailVM()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }





    }
}