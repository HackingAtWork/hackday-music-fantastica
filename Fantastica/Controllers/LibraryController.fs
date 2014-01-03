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
open Fantastica.Api.Entities
open Fantastica.Api

type LibraryController() =
    inherit ApiController()
    
    let mp3s = DataStore.Instance.SongRepository.FindAll()
    
    member x.Get([<FromUri>]filter:LibraryFilter) =
      let filterTitle songs = match filter with
                                | h when not (String.IsNullOrWhiteSpace(h.Title)) 
                                    -> songs |> List.filter (fun (mp3:Song) -> mp3.Title.ToLower().Contains(h.Title.ToLower()))
                                | _ -> songs

      let filterAlbum songs = match filter with
                                | h when not (String.IsNullOrWhiteSpace(h.Album)) 
                                    -> songs |> List.filter (fun (mp3:Song) -> mp3.Album.ToLower().Contains(h.Album.ToLower()))
                                | _ -> songs
     
      let filterArtist songs = match filter with
                                | h when not (String.IsNullOrWhiteSpace(h.Artist)) 
                                    -> songs |> List.filter (fun (mp3:Song) -> mp3.Artist.ToLower().Contains(h.Artist.ToLower()))
                                | _ -> songs

      let filterAlbumArtist songs = match filter with
                                    | h when not (String.IsNullOrWhiteSpace(h.AlbumArtist)) 
                                        -> songs |> List.filter (fun (mp3:Song) -> mp3.AlbumArtist.ToLower().Contains(h.AlbumArtist.ToLower()))
                                    | _ -> songs

      mp3s.ToList() |> Seq.toList |> filterTitle |> filterAlbum |> filterArtist |> filterAlbumArtist

    member x.Put([<FromBody>]songIds:string array)=
        DataStore.Instance.SongRepository.Find(fun (s:Song) -> songIds.Contains(s.Id))