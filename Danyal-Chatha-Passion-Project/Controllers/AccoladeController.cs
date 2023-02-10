using Danyal_Chatha_Passion_Project.Models;
using Danyal_Chatha_Passion_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Danyal_Chatha_Passion_Project.Controllers
{
    public class AccoladeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AccoladeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/");
        }

        // GET: Accolade/List
        public ActionResult List()
        {
            string url = "accoladesdata/listaccolades";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AccoladeDto> Accolades = response.Content.ReadAsAsync<IEnumerable<AccoladeDto>>().Result;
            return View(Accolades);
        }

        // GET: Accolade/Details/5
        public ActionResult Details(int id)
        {
            DetailAccolade ViewModel = new DetailAccolade();

            string url = "accoladesdata/findaccolade/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AccoladeDto SelectedAccolade = response.Content.ReadAsAsync<AccoladeDto>().Result;
            ViewModel.SelectedAccolade = SelectedAccolade;

            url = "playerdata/listplayerforaccolade/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PlayerDto> RewardedPlayer = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;

            ViewModel.RewardedPlayer = RewardedPlayer;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Accolade/New
        public ActionResult New()
        {
            return View();
        }

        //POST: Accolade/Create
        [HttpPost]
        public ActionResult Create(Accolade Accolades)
        {
            string url = "accoladesdata/addaccolade";
            string jsonpayload = jss.Serialize(Accolades);
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


        // GET: Accolade/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "accoladesdata/findaccolade/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AccoladeDto SelectedAccolade = response.Content.ReadAsAsync<AccoladeDto>().Result;
            return View(SelectedAccolade);
        }

        // POST: Accolade/Update/5
        [HttpPost]
        public ActionResult Update(int id, Accolade Accolades)
        {
            string url = "accoladesdata/updateaccolade/" + id;
            string jsonpayload = jss.Serialize(Accolades);
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

        // GET: Accolade/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "accoladesdata/findaccolade/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AccoladeDto SelectedAccolade = response.Content.ReadAsAsync<AccoladeDto>().Result;
            return View(SelectedAccolade);
        }

        // POST: Accolade/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "accoladesdata/deleteaccolade/" + id;
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
