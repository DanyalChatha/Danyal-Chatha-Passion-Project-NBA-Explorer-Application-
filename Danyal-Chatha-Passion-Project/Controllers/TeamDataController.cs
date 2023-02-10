using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Danyal_Chatha_Passion_Project.Models;

namespace Danyal_Chatha_Passion_Project.Controllers
{
    public class TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TeamData/ListTeams
        [HttpGet]
        [ResponseType(typeof(TeamDto))]
        public IHttpActionResult ListTeam()
        {
            List<Team> Team = db.Teams.ToList();
            List<TeamDto> TeamDto = new List<TeamDto>();

            Team.ForEach(T => TeamDto.Add(new TeamDto()
            {
                TeamId = T.TeamId,
                TeamName = T.TeamName,
                TeamBio = T.TeamBio
            }));

            return Ok(TeamDto);
        }

        // GET: api/TeamData/FindTeam/5
        [ResponseType(typeof(TeamDto))]
        [HttpGet]
        public IHttpActionResult FindTeam(int id)
        {
            Team team = db.Teams.Find(id);
            TeamDto TeamDto = new TeamDto()
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                TeamBio = team.TeamBio
            };
            if (team == null)
            {
                return NotFound();
            }

            return Ok(TeamDto);
        }

        // PUT: api/TeamData/UpdateTeam/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTeam(int id, Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.TeamId)
            {
                return BadRequest();
            }

            db.Entry(team).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        // POST: api/TeamData/AddTeam
        [ResponseType(typeof(Team))]
        [HttpPost]
        public IHttpActionResult AddTeam(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teams.Add(team);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = team.TeamId }, team);
        }

        // DELETE: api/TeamData/DeleteTeam/5
        [ResponseType(typeof(Team))]
        [HttpPost]
        public IHttpActionResult DeleteTeam(int id)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }

            db.Teams.Remove(team);
            db.SaveChanges();

            return Ok(team);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.TeamId == id) > 0;
        }
    }
}