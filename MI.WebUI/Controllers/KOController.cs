using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using System.Web.Script.Serialization;
using MI.Web.Infrastructure;

namespace MI.WebUI.Controllers
{
    public class KOController : Controller
    {
        //
        // GET: /KO/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Basics()
        {
            return View();
        }

        public ActionResult Collections()
        {
            return View();
        }

        public ActionResult SinglePageWithSammy()
        {
            return View();
        }

        public ActionResult SinglePage()
        {
            return View();
        }
        public ActionResult CustomBindings()
        {
            return View();
        }

      
        [HttpGet]
        public ActionResult mail(string folder)
        {
            dynamic email = new List<dynamic>();

            email.Add(new ExpandoObject());
            email[0].from= "John Smith" + folder;
            email[0].to = "abc@go.com";
            email[0].subject = "This is something urgent";
            email[0].date= "01/01/2010";
            email[0].id =1;

            email.Add(new ExpandoObject());
            email[1].from = "Paris Hilton";
            email[1].to = "xyzc@go.com";
            email[1].subject = "Want my tapes back";
            email[1].date = "06/01/2010";
            email[1].id = 2;
           
            return VidpubJSON(email);
        }

        [HttpGet]
        public ActionResult onemail(string mailId)
        {
            dynamic email = new ExpandoObject();


            email.from = "John Smith : Id:" + mailId.ToString(); ;
            email.to = "abc@go.com";
            email.subject = "This is something urgent";
            email.date = "01/01/2010";
            email.body = "something";
       

            return VidpubJSON(email);
        }

        public ActionResult VidpubJSON(dynamic content)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoObjectConverter() });
            var json = serializer.Serialize(content);
            Response.ContentType = "application/json";
            return Content(json);
        }


    }//

    class email
    {
        public string  from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string date { get; set; }
        
    }
}//
