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
open Fantastica.Api

type RoomsController() =
    inherit ApiController()

    

    member x.Get([<FromUri>]filter:UserFilter) =
        if(String.IsNullOrWhiteSpace filter.Name) then 
            DataStore.Instance.UserRepository.FindAll()
        else
            DataStore.Instance.UserRepository.Find(fun (u:User) -> u.Name.ToLower().Contains(filter.Name.ToLower()))

type Room() =
    member val DJ = null with get, set
    member val Users = [] with get, set
    member val Name = "Default Room" with get, set