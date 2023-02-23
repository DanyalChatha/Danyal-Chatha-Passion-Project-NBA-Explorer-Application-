using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Danyal_Chatha_Passion_Project.Models;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Danyal_Chatha_Passion_Project.Controllers
{
    public class TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns a list of all the teams in the database
        /// </summary>
        /// <returns>
        /// </returns>
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
                TeamBio = T.TeamBio,
                TeamHasPic = T.TeamHasPic,
                TeamPicExtension = T.TeamPicExtension,
            }));

            return Ok(TeamDto);
        }

        /// <summary>
        /// Returns all teams in the system
        /// </summary>
        /// <param name="id">The primary key of the team</param>
        /// <returns>
        /// Content: An Team in the system matching up to the Team Id.
        /// </returns>
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
                TeamBio = team.TeamBio,
                TeamHasPic = team.TeamHasPic,
                TeamPicExtension = team.TeamPicExtension
            };
            if (team == null)
            {
                return NotFound();
            }

            return Ok(TeamDto);
        }
        /// <summary>
        /// Able to update the team in the system with the matching id 
        /// </summary>
        /// <param name="id">The Primary key of the team</param>
        /// <param name="team">JSON FORM DATA of an teams</param>
        /// <returns></returns>
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
            //Picture update is handled by another method
            db.Entry(team).Property(T => T.TeamHasPic).IsModified = false;
            db.Entry(team).Property(T => T.TeamHasPic).IsModified = false;

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

        //POST: api/TeamData/Updateteampic/3
        [HttpPost]
        public IHttpActionResult UploadTeamPic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                int numfiles = HttpContext.Current.Request.Files.Count;

                //Check if file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var teamPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (teamPic.ContentLength > 0)
                    {
                        //Establish valid file type (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(teamPic.FileName).Substring(1);
                        //Check the extension of the file 
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/teams/{id}.{extensions}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Teams/"), fn);
                                
                                //save the file
                                
                                teamPic.SaveAs(path);
                                
                                //if these are all successful than we can set these fields.
                                haspic = true;
                                picextension = extension;
                                
                                //Update the team haspic and picextension fields in the database
                                Team SelectedTeam = db.Teams.Find(id);
                                SelectedTeam.TeamHasPic = haspic;
                                SelectedTeam.TeamPicExtension = picextension;
                                db.Entry(SelectedTeam).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }
                }
                return Ok();
            }
            else
            {
                //Not multipart data
                return BadRequest();
            }
        }   

        /// <summary>
        /// Add a team to the database
        /// </summary>
        /// <param name="team">JSON FORM DATA of an team</param>
        /// <returns>
        /// Content: Team Id & Team Data
        /// </returns>
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
        /// <summary>
        /// Delete a team with the matching id that has been requested
        /// </summary>
        /// <param name="id">The Primary Key of team</param>
        /// <returns>
        /// </returns>
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

            if (team.TeamHasPic && team.TeamPicExtension != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Teams/" + id + "." + team.TeamPicExtension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
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