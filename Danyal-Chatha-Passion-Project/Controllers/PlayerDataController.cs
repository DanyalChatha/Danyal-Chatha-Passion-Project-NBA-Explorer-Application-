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