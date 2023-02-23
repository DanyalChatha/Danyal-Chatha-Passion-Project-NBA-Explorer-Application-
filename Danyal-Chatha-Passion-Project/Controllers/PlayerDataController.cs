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
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace Danyal_Chatha_Passion_Project.Controllers
{
    public class PlayerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns a list of players
        /// </summary>
        /// <returns>
        /// Content: all players in the database, including the team they play on.
        /// </returns>
        // GET: api/PlayerData/ListPlayers
        [HttpGet]
        [ResponseType(typeof(PlayerDto))]
        public IHttpActionResult ListPlayers()
        {
            List<Player> Players = db.Players.ToList();
            List<PlayerDto> playerDtos = new List<PlayerDto>();

            Players.ForEach(a => playerDtos.Add(new PlayerDto(){
                PlayerId = a.PlayerId,
                PlayerName = a.PlayerName,
                PlayerJersey = a.PlayerJersey,
                PlayerPosition = a.PlayerPosition,
                PlayerHasPic = a.PlayerHasPic,
                PicExtension = a.PicExtension,
                TeamName = a.Team.TeamName

            }));
                return Ok(playerDtos);
        }

        /// <summary>
        /// Gathers info about all players related to a certain team Id
        /// </summary>
        /// <param name="id">Team Id</param>
        /// <returns>
        /// content: all players in the databse, including the team they play on according to the team id
        /// </returns>
        //GET: api/PlayerData/ListPlayerforTeam/5
        [HttpGet]
        [ResponseType(typeof(PlayerDto))]
        public IHttpActionResult ListPlayersForTeam(int id)
        {
            List<Player> Players = db.Players.Where(a=>a.TeamId==id).ToList();
            List<PlayerDto> playerDtos = new List<PlayerDto>();

            Players.ForEach(a => playerDtos.Add(new PlayerDto()
            {
                PlayerId = a.PlayerId,
                PlayerName = a.PlayerName,
                PlayerJersey = a.PlayerJersey,
                PlayerPosition = a.PlayerPosition,
                TeamId = a.TeamId,
                TeamName = a.Team.TeamName

            }));

            return Ok(playerDtos);

        }
        /// <summary>
        /// Inquire info about all players related to a certain accolade.
        /// </summary>
        /// <param name="id">Accolade Id</param>
        /// <returns>
        /// Content: all players in the database including thier rewarded accolade according to match a certain accolade id.
        /// </returns>
        //GET: api/PlayerData/ListPlayerforAccoldade/5
        [HttpGet]
        [ResponseType(typeof(PlayerDto))]
        public IHttpActionResult ListPlayerForAccolade(int id)
        {
            List<Player> Players = db.Players.Where(
                a=>a.Accolades.Any(
                    p =>p.AccoladeId==id
                    )).ToList();
            List<PlayerDto> playerDtos = new List<PlayerDto>();

            Players.ForEach(a => playerDtos.Add(new PlayerDto()
            {   
                PlayerId=a.PlayerId,
                PlayerName=a.PlayerName,
                PlayerJersey=a.PlayerJersey,
                PlayerPosition=a.PlayerPosition,
                TeamId=a.Team.TeamId,
                TeamName=a.Team.TeamName

            }));

            return Ok(playerDtos);
        }

        /// <summary>
        /// Associate a certain accolade with a certain team.
        /// </summary>
        /// <param name="playerid">The Player Id primary key</param>
        /// <param name="accoladeid">The Accolade Id primary key</param>
        /// <returns>
        /// </returns>
        //GET: api/PlayerData/AssociatePlayerWithAccolade/1/4
        [HttpPost]
        [Route("api/PlayerData/AssociatePlayerWithAccolade/{playerid}/{accoladeid}")]
        public IHttpActionResult AssociatePlayerWithAccolade(int playerid, int accoladeid)
        {
            Player SelectedPlayer = db.Players.Include(a => a.Accolades).Where(a => a.PlayerId == playerid).FirstOrDefault();
            Accolade SelectedAccolade = db.Accolades.Find(accoladeid);

            if (SelectedPlayer == null || SelectedAccolade == null)
            {
                return NotFound();
            }

            SelectedPlayer.Accolades.Add(SelectedAccolade);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes a associated accolade from the particular player
        /// </summary>
        /// <param name="playerid">The Player Id primary key</param>
        /// <param name="accoladeid">The Accolade Id primary key</param>
        /// <returns></returns>
        //POST: api/PlayerData/UnAssociatePlayerWithAccolade/1/4
        [HttpPost]
        [Route("api/PlayerData/UnAssociatePlayerWithAccolade/{playerid}/{accoladeid}")]
        public IHttpActionResult UnAssociatePlayerWithAccolade(int playerid, int accoladeid)
        {
            Player SelectedPlayer = db.Players.Include(a => a.Accolades).Where(a => a.PlayerId == playerid).FirstOrDefault();
            Accolade SelectedAccolade = db.Accolades.Find(accoladeid);

            if (SelectedPlayer == null || SelectedAccolade == null)
            {
                return NotFound();
            }

            SelectedPlayer.Accolades.Remove(SelectedAccolade);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Return all players in the system
        /// </summary>
        /// <param name="id">The Player Id primary key</param>
        /// <returns>
        /// Content: A player in the system matching up to the player Id primary key
        /// </returns>
        // GET: api/PlayerData/FindPlayer/5
        [ResponseType(typeof(PlayerDto))]
        [HttpGet]

        public IHttpActionResult FindPlayer(int id)
        {
            Player player = db.Players.Find(id);
            PlayerDto playerDto = new PlayerDto()
            {
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName,
                PlayerJersey = player.PlayerJersey,
                PlayerPosition = player.PlayerPosition,
                PlayerHasPic = player.PlayerHasPic,
                PicExtension = player.PicExtension,
                TeamName = player.Team.TeamName
            };

            if (player == null)
            {
                return NotFound();
            }

            return Ok(playerDto);
        }

        /// <summary>
        /// Updates a player according to requested player id.
        /// </summary>
        /// <param name="id">Represent the player Id primary key </param>
        /// <param name="player">JSON FORM DATA of a player </param>
        /// <returns></returns>
        // POST: api/PlayerData/UpdatePlayer/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePlayer(int id, Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player.PlayerId)
            {
                return BadRequest();
            }

            db.Entry(player).State = EntityState.Modified;
            //Picture update is handeled by another method
            db.Entry(player).Property(a => a.PlayerHasPic).IsModified = false;
            db.Entry(player).Property(a => a.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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


        //POST: api/playerdata/UpdateplayerPic/1
        [HttpPost]
        public IHttpActionResult UploadPlayerPic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                int numfiles = HttpContext.Current.Request.Files.Count;

                //Checks if files is posted
                if(numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var playerPic = HttpContext.Current.Request.Files[0];
                    //Check if files is empty
                    if (playerPic.ContentLength > 0)
                    {
                        //Establish valid files types
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(playerPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Players/"), fn);

                                //Save the file
                                playerPic.SaveAs(path);

                                //if these are all successful than we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the player haspic and picextension fields in the database
                                Player SelectedPlayer = db.Players.Find(id);
                                SelectedPlayer.PlayerHasPic = haspic;
                                SelectedPlayer.PicExtension = extension;
                                db.Entry(SelectedPlayer).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch  (Exception ex)
                            {
                                Debug.WriteLine("Images was not saved successfully");
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
                //Not multipart from data
                return BadRequest();
            }
        }

        /// <summary>
        /// Add a player to the database
        /// </summary>
        /// <param name="player"> JSON FORM DATA of a player </param>
        /// <returns>
        /// Content: Player Id & PlayerData
        /// </returns>
        // POST: api/PlayerData/AddPlayer
        [ResponseType(typeof(Player))]
        [HttpPost]
        public IHttpActionResult AddPlayer(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Players.Add(player);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = player.PlayerId }, player);
        }

        /// <summary>
        /// Delete/Removes a player from the database
        /// </summary>
        /// <param name="id">The Primary Key of the player</param>
        /// <returns></returns>
        // POST: api/PlayerData/DeletePlayer/5
        [ResponseType(typeof(Player))]
        [HttpPost]
        public IHttpActionResult DeletePlayer(int id)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }

            if (player.PlayerHasPic && player.PicExtension != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Players/" + id + "." + player.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            db.Players.Remove(player);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(int id)
        {
            return db.Players.Count(e => e.PlayerId == id) > 0;
        }
    }
}