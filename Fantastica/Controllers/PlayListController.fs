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
    
    /// Gets all values.
    [<Route("")>]
    member x.Get() = 
      let path = @"C:\Development\hackday-music-fantastica\Fantastica\Content\mp3s"
      let mp3s= TagReader.getAllId3v2ValidTags (TagReader.getAllMp3Files path)
      mp3s |> List.map (fun s -> {SongTitle=s.Title; Artist=s.JoinedPerformers; Album=s.Album; AlbumArtist=s.JoinedAlbumArtists})



