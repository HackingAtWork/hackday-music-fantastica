module TagReader

open System.IO
open TagLib
open TagLib.Mpeg
open System
open Fantastica.Api.Entities
open Fantastica.Models

let getAllMp3Files(path:string) =
    let dir = new DirectoryInfo(path)
    dir.GetFiles("*.mp3")
        |> Array.map (fun t -> (t.Name, t.FullName))
        |> Array.toList

let getAllId3v2ValidTags(mp3List) =
    let getId3v2Tag((mp3file:string,mp3path:string)) =
        let tagEngine = new TagLib.Mpeg.AudioFile(mp3path)
        let tag = tagEngine.GetTag(TagTypes.Id3v2)
        if (tag = null) then None
        else Some((tag,mp3file))

    mp3List |> List.map (fun mp3 -> getId3v2Tag mp3)
            |> List.choose id

let filterSongs(mp3s:Song list, filter:LibraryFilter) : Song list =
    let acceptOrFilter (field:string, filter:string) = 
        String.IsNullOrWhiteSpace(filter) 
            || field.ToLower().Contains(filter.ToLower())
    let songfilter (song:Song) =
        acceptOrFilter(song.Album, filter.Album)
            && acceptOrFilter(song.AlbumArtist, filter.AlbumArtist)
            && acceptOrFilter(song.Artist, filter.Artist)
            && acceptOrFilter(song.Title, filter.Title)

    List.filter songfilter mp3s