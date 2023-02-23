using Danyal_Chatha_Passion_Project.Models;
using Danyal_Chatha_Passion_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Danyal_Chatha_Passion_Project.Controllers
{
    public class TeamController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TeamController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/");
        }
        // GET: Team/List
        public ActionResult List()
        {
            string url = "teamdata/listteam";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TeamDto> teams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            return View(teams);
        }

        // GET: Team/Details/5
        public ActionResult Details(int id)
        {
            DetailsTeam ViewModel = new DetailsTeam();

            string url = "teamdata/findteam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
            ViewModel.SelectedTeam = SelectedTeam ;

            url = "playerdata/listplayersforteam/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PlayerDto> RelatedPlayers = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;
            ViewModel.RelatedPlayers = RelatedPlayers;
            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Team/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        public ActionResult Create(Team team)
        {
            string url = "teamdata/addteam";
            string jsonpayload = jss.Serialize(team);
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

        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "teamdata/findteam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
            return View(SelectedTeam);
        }

        // POST: Team/Update/5
        [HttpPost]
        public ActionResult Update(int id, Team Team, HttpPostedFileBase TeamPic)
        {
            
            string url = "teamdata/updateteam/" + id;
            string jsonpayload = jss.Serialize(Team);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //Update request is successful, and we have image data
            if (response.IsSuccessStatusCode && TeamPic != null)
            {
                //send over image data for team
                url = "TeamData/UploadTeamPic/" + id;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(TeamPic.InputStream);
                requestcontent.Add(imagecontent, "TeamPic", TeamPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Team/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "teamdata/findteam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
            return View(SelectedTeam);
        }

        // POST: Team/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "teamdata/deleteteam/" + id;
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
