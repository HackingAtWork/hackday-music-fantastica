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

type LibraryController() =
    inherit ApiController()
    let path= HttpContext.Current.Server.MapPath("~/Content/mp3s")
    let basePathLength = HttpContext.Current.Server.MapPath("~").Length + 1
    let mp3s= TagReader.getAllId3v2ValidTags (TagReader.getAllMp3Files path)
              |> List.map (fun (s,file) -> 
                    {SongTitle=s.Title;Artist=s.JoinedPerformers;AlbumArtist=s.JoinedAlbumArtists;
                     Album=s.Album;Path=System.IO.Path.Combine(path,file).Substring(basePathLength).Replace('\\','/')})
    
    member x.Get([<FromUri>]filter:LibraryFilter) =

      let filterTitle songs = match filter with
                                            | h when not (String.IsNullOrWhiteSpace(h.Title)) 
                                                -> songs |> List.filter (fun mp3 -> mp3.SongTitle.ToLower().Contains(h.Title.ToLower()))
                                            | _ -> songs

      let filterAlbum songs = match filter with
                                            | h when not (String.IsNullOrWhiteSpace(h.Album)) 
                                                -> songs |> List.filter (fun mp3 -> mp3.Album.ToLower().Contains(h.Album.ToLower()))
                                            | _ -> songs
     
      let filterArtist songs = match filter with
                                            | h when not (String.IsNullOrWhiteSpace(h.Artist)) 
                                                -> songs |> List.filter (fun mp3 -> mp3.Artist.ToLower().Contains(h.Artist.ToLower()))
                                            | _ -> songs

      let filterAlbumArtist songs = match filter with
                                            | h when not (String.IsNullOrWhiteSpace(h.AlbumArtist)) 
                                                -> songs |> List.filter (fun mp3 -> mp3.AlbumArtist.ToLower().Contains(h.AlbumArtist.ToLower()))
                                            | _ -> songs

      mp3s |> filterTitle |> filterAlbum |> filterArtist |> filterAlbumArtist
