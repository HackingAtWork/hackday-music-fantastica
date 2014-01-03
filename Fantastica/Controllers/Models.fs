namespace Fantastica.Models

open System
open System.Runtime.Serialization

type LibraryFilter() =
    member val Title = "" with get, set
    member val Album  = "" with get, set
    member val Artist = "" with get, set
    member val AlbumArtist  = "" with get, set

type UserFilter() =
    member val Name = "" with get,set


