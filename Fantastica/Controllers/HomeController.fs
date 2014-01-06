namespace Fantastica.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open System.Web.Mvc.Ajax
open System.IO
open Fantastica.Api

type HomeController() =
    inherit Controller()
    member this.Index () = this.View()

    member this.NextSong() = 
        let rnd = new Random()
        
        let mp3s = DataStore.Instance.SongRepository.FindAll()
        
        this.Json(mp3s.First(fun s -> s.Id.Equals("songs-" + rnd.Next(1, mp3s.Count()-1).ToString())),JsonRequestBehavior.AllowGet)
        //allFiles.[rnd.Next(0,allFiles.Count()-1)]

