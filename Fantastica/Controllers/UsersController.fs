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

    member x.Get() =
        let u = new User(Name = "Test User")
        saveUser u |> ignore

        getAllUsers

