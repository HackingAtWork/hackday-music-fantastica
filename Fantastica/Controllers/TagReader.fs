module TagReader

open System.IO
open TagLib
open TagLib.Mpeg
open System
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

let filterSongs(mp3s, filter:LibraryFilter) : Song list =
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