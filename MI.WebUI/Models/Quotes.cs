using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MI.Web.Model
{
    public class Quotes
    {
        public static dynamic FromUsers()
        {
            var quotes = new string[]
            {
                @"<h4>Uses Massive</h4> This framework uses Massive for database interaction"
                ,"<h4> Crud </h4>Has built in Search, CRUD, Comboboxes ... with REST."
                ,"<h4> JQuery UI themes </h4>Has multiple themes for the Grid. In shared_layout.cshtml change the tag : @Assets.jgrid() to any of these:dot-luv, humanity, redmond,start,ui-darkness,ui-lightness,blitzer, dark-hive,eggplant,le-frog,  mint-choc,south-street,trontastic"
               ,  "Hooked up to the Blueprint css system."
               
            };

            Random rnd = new Random();

            return quotes.OrderBy(x=> rnd.Next()).ToList();
        }
    }//
}//