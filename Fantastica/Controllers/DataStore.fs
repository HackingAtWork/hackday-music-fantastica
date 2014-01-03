module DataStore

open Fantastica.Api
open Fantastica.Api.Entities
open System.Linq
open System.Web
open System.Web.Mvc

let buildSongs =  
    let path= HttpContext.Current.Server.MapPath("~/Content/mp3s")
    let basePathLength = HttpContext.Current.Server.MapPath("~").Length + 1
    let mp3s= TagReader.getAllId3v2ValidTags (TagReader.getAllMp3Files path)
              |> List.map (fun (s,file) -> 
                   new Song(Title=s.Title,Artist=s.JoinedPerformers,AlbumArtist=s.JoinedAlbumArtists,
                     Album=s.Album,Path=System.IO.Path.Combine(path,file).Substring(basePathLength).Replace('\\','/')))
    DataStore.Instance.SongRepository.ExclusiveBulkSave(mp3s) |> ignore