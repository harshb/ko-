using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//in web.config in Views
// <pages pageBaseType="VidPub.Web.Infrastructure.VibPubViewPage">

//Usage: @Routes.about_path()

//Phil hack post: http://haacked.com/archive/2011/02/21/changing-base-type-of-a-razor-view.aspx

namespace MI.Web.Infrastructure
{
   //typed
    public class VibPubViewPage<TModel>:WebViewPage<TModel>
    {
        public dynamic Routes { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            Routes = new RouteHelper(Url);
        }
        public override void Execute()
        {
            base.ExecutePageHierarchy();
        }
    }
}