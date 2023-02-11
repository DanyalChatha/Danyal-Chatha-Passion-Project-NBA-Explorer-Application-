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
        /// <summary>
        /// Return a list of accolades
        /// </summary>
        /// <returns>
        /// All Accolades in the database are returned.
        /// </returns>
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
        /// <summary>
        /// Return all accolades in the system with an associated player
        /// </summary>
        /// <param name="id">The Primary key of the Player </param>
        /// <returns>
        /// All accolades that have been rewarded to a particular player
        /// </returns>
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
        /// <summary>
        /// Return Accolades in the system not rewarded to the particular player.
        /// </summary>
        /// <param name="id">The Primary key of the Player</param>
        /// <returns>
        /// Content: all accolade in the database 
        /// </returns>
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
        /// <summary>
        /// Return all accolade in the system associated with a particular player
        /// </summary>
        /// <param name="id">Accolade Primary key</param>
        /// <returns>
        /// Content: A accolade in the system matching the requested id 
        /// </returns>
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

        /// <summary>
        /// Updates the Accolade with a matching id 
        /// </summary>
        /// <param name="id">TThe Primary key of the Accolade</param>
        /// <param name="Accolades">JSON FORM DATA of an Accolade</param>
        /// <returns></returns>
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
        /// <summary>
        /// Add an Accolade to the database
        /// </summary>
        /// <param name="Accolades">JSON FORM DATA of an accolade</param>
        /// <returns>
        /// Content: Accolade Id & Accolade Data
        /// </returns>
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
            
        
        /// <summary>
        /// Delete the Accolade that matched with the requested id 
        /// </summary>
        /// <param name="id">The Primary key of the Accolade</param>
        /// <returns></returns>
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