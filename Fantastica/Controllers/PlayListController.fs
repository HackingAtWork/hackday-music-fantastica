namespace Fantastica.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open System.Web.Mvc.Ajax
open System.Net;
open System.Web.Http;
open Fantastica.Models;

[<RoutePrefix("api2/playlist")>]
type PlayListController() =
    inherit ApiController()
    let songs =  [|{SongTitle="Black"; Artist="Me"; Album="Pearl Jam"};
                   {SongTitle="Doesnt Matter"; Artist="Maximus"; Album="Hackday special"};
                   {SongTitle="zelda_theme"; Artist="Some dude"; Album="Zelda Soundtrack"};
                   {SongTitle="street_fighter_theme"; Artist="Someone's Mama"; Album="Street Fighter SOundtrack"};|]
    
    /// Gets all values.
    [<Route("")>]
    member x.Get() = songs


