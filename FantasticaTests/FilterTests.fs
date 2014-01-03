namespace FantasticaTests

open NUnit.Framework
open FSharp.Data
open TagReader
open Fantastica.Models
open Fantastica.Api.Entities

[<TestFixture>]
type FilterTests() = 
    let songs = [
        new Song(Title = "Hit me baby", 
            Album = "oh yeah", 
            Artist = "nsync",
            Path = "../",
            AlbumArtist = "nsync"
        ); 
        new Song(Title = "baby for you", 
            Album = "black", 
            Artist = "santana",
            AlbumArtist = "santana",
            Path = "../")
    ]
      
    [<TestCase("nsync", "oh")>]
    [<TestCase("nsync", "me")>]
    [<TestCase("nsync", "nsy")>]
    [<TestCase("nsync", "sync")>]
    [<TestCase("santana", "san")>]
    member tc.TestLibs(expectedartist, filter) =
        let x = TagReader.filterSongs(songs, filter)
        Assert.AreEqual(expectedartist, x.Head.Artist)
        