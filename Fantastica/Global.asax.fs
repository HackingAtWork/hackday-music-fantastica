namespace Fantastica

open System
open System.Net.Http
open System.Web
open System.Web.Http
open System.Web.Mvc
open System.Web.Routing
open System.Web.Optimization
open System.Linq
open DataStore
open Fantastica.Api.Entities
open Fantastica.Api

type BundleConfig() =
    static member RegisterBundles (bundles:BundleCollection) =
        bundles.Add(ScriptBundle("~/bundles/JS").Include([|"~/Scripts/*.js"|]))

        bundles.Add(StyleBundle("~/bundles/CSS").Include([|"~/Content/*.css"|]))

/// Route for ASP.NET MVC applications
type Route = { 
    controller : string
    action : string
    id : UrlParameter }

type HttpRoute = {
    controller : string
    id : RouteParameter }

type Global() =
    inherit System.Web.HttpApplication() 

    static member RegisterWebApi(config: HttpConfiguration) =
        // Configure routing
        config.MapHttpAttributeRoutes()
        let appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(fun t -> t.MediaType = "application/xml");
        let j=config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        
        config.Routes.MapHttpRoute(
            "DefaultApi", // Route name
            "api/{controller}/{id}", // URL with parameters
            { controller = "{controller}"; id = RouteParameter.Optional } // Parameter defaults
        ) |> ignore
       

    static member RegisterFilters(filters: GlobalFilterCollection) =
        filters.Add(new HandleErrorAttribute())

    static member RegisterRoutes(routes:RouteCollection) =
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            { controller = "Home"; action = "Index"; id = UrlParameter.Optional } // Parameter defaults
        ) |> ignore

    member x.Application_Start() =
        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(Action<_> Global.RegisterWebApi)
        Global.RegisterFilters(GlobalFilters.Filters)
        Global.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles BundleTable.Bundles
        
        
        let playList= new PlayList(Name="fuzzy",SongIds=[ "hey"; "hey12"])
        let u = new User(Name = "Test User", Lists=[playList])
        DataStore.Instance.UserRepository.Save u |> ignore

        buildSongs
