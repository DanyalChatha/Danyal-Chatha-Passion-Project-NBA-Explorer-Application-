using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Danyal_Chatha_Passion_Project.Models;

namespace Danyal_Chatha_Passion_Project.Controllers
{
    public class AccoladesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AccoladesData/ListAccolades
        [HttpGet]
        [ResponseType(typeof(AccoladeDto))]
        public IHttpActionResult ListAccolades()
        {
            List<Accolade> Accolade = db.Accolades.ToList();
            List<AccoladeDto> AccoladeDtos = new List<AccoladeDto>();

            Accolade.ForEach(a =>AccoladeDtos.Add(new AccoladeDto()
            {
                AccoladeId = a.AccoladeId,
                AccoladeName = a.AccoladeName,
                AccoladeYear = a.AccoladeYear
            }));

            return Ok(AccoladeDtos);
        }

        // GET: api/AccoladesData/ListAccoladeForPlayer/5
        [HttpGet]
        [ResponseType(typeof(AccoladeDto))]
        public IHttpActionResult ListAccoladeForPlayer(int id)
        {
            List<Accolade> Accolades = db.Accolades.Where(
                a => a.Players.Any(
                    p => p.PlayerId == id)
                ).ToList();
            List<AccoladeDto> AccoladeDtos = new List<AccoladeDto>();

            Accolades.ForEach(a => AccoladeDtos.Add(new AccoladeDto()
            {
                AccoladeId = a.AccoladeId,
                AccoladeName = a.AccoladeName,
                AccoladeYear = a.AccoladeYear
            }));

            return Ok(AccoladeDtos);
        }
        // GET: api/AccoladesData/ListAccoladeNotForPlayer/5
        [HttpGet]
        [ResponseType(typeof(AccoladeDto))]
        public IHttpActionResult ListAccoladeNotForPlayer(int id)
        {
            List<Accolade> Accolades = db.Accolades.Where(
                a => !a.Players.Any(
                    p => p.PlayerId == id)
                ).ToList();
            List<AccoladeDto> AccoladeDtos = new List<AccoladeDto>();

            Accolades.ForEach(a => AccoladeDtos.Add(new AccoladeDto()
            {
                AccoladeId = a.AccoladeId,
                AccoladeName = a.AccoladeName,
                AccoladeYear = a.AccoladeYear
            }));

            return Ok(AccoladeDtos);
        }

        //GET: api/AccoladesData/FindAccolade/5
        [ResponseType(typeof(AccoladeDto))]
        [HttpGet]
        public IHttpActionResult FindAccolade(int id)
        {
            Accolade Accolade = db.Accolades.Find(id);
            AccoladeDto AccoladeDto = new AccoladeDto()
            {
                AccoladeId = Accolade.AccoladeId,
                AccoladeName = Accolade.AccoladeName,
                AccoladeYear = Accolade.AccoladeYear
            };
            if (Accolade == null)
            {
                return NotFound();
            }   

            return Ok(AccoladeDto);
        }

        //POST: api/AccoladesData/UpdateAccolade/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAccolade(int id, Accolade Accolades)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Accolades.AccoladeId)
            {
                //Can't seem to fix the POST to match with the GET id.
                Debug.WriteLine("GET paramater:" + id);
                Debug.WriteLine("POST paramater:" + Accolades.AccoladeId);
                return BadRequest();
            }

            db.Entry(Accolades).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccoladeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        //POST: api/AccoladesData/AddAccolade
        [ResponseType(typeof(Accolade))]
        [HttpPost]

        public IHttpActionResult AddAccolade(Accolade Accolades)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Accolades.Add(Accolades);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Accolades.AccoladeId }, Accolades);
        }   
            
        

        // DELETE: api/AccoladesData/DeleteAccolade/5
        [ResponseType(typeof(Accolade))]
        [HttpPost]
        public IHttpActionResult DeleteAccolade(int id)
        {
            Accolade Accolade = db.Accolades.Find(id);
            if (Accolade == null)
            {
                return NotFound();
            }

            db.Accolades.Remove(Accolade);
            db.SaveChanges();

            return Ok(Accolade);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccoladeExists(int id)
        {
            return db.Accolades.Count(e => e.AccoladeId == id) > 0;
        }
    }
}