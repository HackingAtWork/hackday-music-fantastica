namespace FantasticaTests

open NUnit.Framework
open FSharp.Data
open TagReader
open Fantastica.Models

[<TestFixture>]
type FilterTests() = 
    let songs = [
        {   Song.SongTitle = "Hit me baby"; 
            Album = "oh yeah"; 
            Artist = "nsync";
            Path = "../"
            AlbumArtist = "nsync"
        }; {   
            Song.SongTitle = "baby for you"; 
            Album = "black"; 
            Artist = "santana";
            AlbumArtist = "santana";
            Path = "../";}
    ]
      
    [<TestCase("nsync", "", "oh" ,"", "")>]
    [<TestCase("nsync", "me", "" ,"", "")>]
    [<TestCase("nsync", "", "" ,"nsy", "")>]
    [<TestCase("nsync", "", "" ,"", "sync")>]
    [<TestCase("santana", "", "" ,"san", "san")>]
    member tc.TestLibs(expectedartist, title, album, artist, albumartist) =
        let filter = new LibraryFilter(Title = title, Album  = album, Artist = artist, AlbumArtist = albumartist)
        
        let x = TagReader.filterSongs(songs, filter)
        Assert.AreEqual(expectedartist, x.Head.Artist)
        