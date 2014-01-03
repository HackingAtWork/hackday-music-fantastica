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
open DataStore
open Fantastica.Api.Entities

type UsersController() =
    inherit ApiController()

    member x.Get([<FromUri>]filter:UserFilter) =
        let playList= new PlayList(Name="fuzzy",SongIds=[ "hey"; "hey12"])
        let u = new User(Name = "Test User", Lists=[playList])
        saveUser u |> ignore

        if(String.IsNullOrWhiteSpace filter.Name) then 
            getAllUsers
        else
          getUserByName filter.Name

