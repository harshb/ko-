using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Helpers;
using System.Web.Mvc.Html;

//Usage
// dynamic router = new System.Web.Mvc.RouteHelper(Url);
// @router.about_url() or  @router.about_url
// @router.about_path()
// @router.about_url(new { id = "Harsh" })


namespace System.Web.Mvc
{

    public class RouteHelper : DynamicObject
    {
        UrlHelper _helper;
        public RouteHelper(UrlHelper helper)
        {
            _helper = helper;
        }
       
        //
    //TryGetMember : Called when Get method of a Property is called. If returned true the value in result will be returned.  

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            result = GetUrl(name, new object[0]);
            return true;
        }
        //TryInvokeMember : Called when a delegate/method is called. The result will be returned.
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var name = binder.Name;
            result = GetUrl(name, args);

            return true;
        }
        string GetUrl(string name, object[] args)
        {
            var result = "";

            //parse the name
            var stems = name.Split('_');

            //the name of the route is the first stem
            //for now - only supporting single word route names
            var routeName = stems[0];

            //get the URL
            var url = "";

            //this is lame - there's a bug in here that is translating the wrong
            //object
            if (args.Length > 0)
            {
                if (args[0].GetType().IsPrimitive)
                {
                    url = _helper.RouteUrl(routeName, new { id = args[0] });
                }
                else
                {
                    url = _helper.RouteUrl(routeName, args[0]);

                }
            }
            else
            {
                url = _helper.RouteUrl(routeName);
            }

            //by convention, the name should end in "url" or "path"
            if (stems.Last() == "url")
            {
                //this is an absolute URL
                result = SiteRoot(false) + url;
            }
            else
            {
                //this is a relative
                result = url;
            }
            return result;
        }

        public string SiteRoot(bool includeAppPath = true)
        {
            var context = _helper.RequestContext.HttpContext;
            var Port = context.Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;
            var Protocol = context.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            var appPath = "";
            if (includeAppPath)
            {
                appPath = context.Request.ApplicationPath;
                if (appPath == "/")
                    appPath = "";
            }
            var sOut = Protocol + context.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            return sOut;
        }
    }
}