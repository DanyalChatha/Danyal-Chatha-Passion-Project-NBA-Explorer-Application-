using Danyal_Chatha_Passion_Project.Models;
using Danyal_Chatha_Passion_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static System.Net.WebRequestMethods;

namespace Danyal_Chatha_Passion_Project.Controllers
{
    public class PlayerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static PlayerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/");
        }

        // GET: Player/List
        public ActionResult List()
        {
            //Objective: communicate with our player data api to retrive a list of player
                        
            string url = "playerdata/listplayers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PlayerDto> players = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;

            return View(players);
        }

        // GET: Player/Details/5
        public ActionResult Details(int id)
        {
            DetailsPlayer ViewModel = new DetailsPlayer();

            //Objective: communicate with our player data api to retrive one player            
            string url = "playerdata/findplayer/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;

            ViewModel.SelectedPlayer = SelectedPlayer;

            //SHOW ASSOCIATED ACCOLADE
            url = "accoladesdata/listaccoladeforplayer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AccoladeDto> AquiredAccolades = response.Content.ReadAsAsync<IEnumerable<AccoladeDto>>().Result;

            ViewModel.AquiredAccolades = AquiredAccolades;

            //SHOW NONASSOCIATED ACCOLADE
            url = "accoladesdata/listaccoladenotforplayer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AccoladeDto> AvailableAccolade = response.Content.ReadAsAsync<IEnumerable<AccoladeDto>>().Result;

            ViewModel.AvailableAccolade = AvailableAccolade;


            return View(ViewModel);
        }

        //POST: Player/Associate/{playerId}
        [HttpPost]
        public ActionResult Associate(int id, int AccoladeId)
        {
            string url = "playerdata/associateplayerwithaccolade/" + id + "/" + AccoladeId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //GET: Player/UnAssociate/{id}?AccoladeId={AccoladeId}
        [HttpGet]
        public ActionResult UnAssociate(int id, int AccoladeId)
        {
            string url = "playerdata/unassociateplayerwithaccolade/" + id + "/" + AccoladeId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Player/New
        public ActionResult New()
        {
            string url = "teamdata/listteam";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> TeamOptions = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
            return View(TeamOptions);
        }

        // POST: Player/Create
        [HttpPost]
        public ActionResult Create(Player player)
        {
            string url = "playerdata/addplayer";            
            string jsonpayload = jss.Serialize(player);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Can't reach list v4");
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Edit/5
        public ActionResult Edit(int id)
        {
            UpdatePlayer ViewModel = new UpdatePlayer();

            string url = "playerdata/findplayer/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;
            ViewModel.SelectedPlayer = SelectedPlayer;

            url = "teamdata/listteam/";
            response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> TeamOptions = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
            ViewModel.TeamOptions = TeamOptions;    

            return View(ViewModel);
        }

        // POST: Player/Update/5
        [HttpPost]
        public ActionResult Update(int id, Player player)
        {
            string url = "playerdata/updateplayer/"+id;
            string jsonpayload = jss.Serialize(player);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }                
            
        }

        // GET: Player/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "playerdata/findplayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto selectedplayer = response.Content.ReadAsAsync<PlayerDto>().Result;

            return View(selectedplayer);
        }

        // POST: Player/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "playerdata/deleteplayer/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }
    }
}
