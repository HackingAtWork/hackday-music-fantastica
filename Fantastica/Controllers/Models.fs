namespace Fantastica.Models

open System
open System.Runtime.Serialization

[<DataContract>]
type Song= { 
        [<field: DataMember(Name="Title")>]
        SongTitle:String;
        
        [<field: DataMember(Name="Artist")>]
        Artist:String; 

        [<field: DataMember(Name="AlbumArtist")>]
        AlbumArtist:String; 
        
        [<field: DataMember(Name="Album")>]
        Album:String;

        [<field: DataMember(Name="Path")>]
        Path:String
        }

type LibraryFilter() =
    member val Title = "" with get, set
    member val Album  = "" with get, set
    member val Artist = "" with get, set
    member val AlbumArtist  = "" with get, set



