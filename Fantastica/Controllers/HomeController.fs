namespace FSharpWeb1.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open System.Web.Mvc.Ajax
open System.IO

type HomeController() =
    inherit Controller()
    member this.Index () = this.View()

    member this.NextSong() = 
        let allFiles = Directory.GetFiles(@"C:\Development\hackday-music-fantastica\Fantastica\Content\mp3s")
                        |> Array.map (fun x -> Path.GetFileName x)
        let rnd = new Random()
        allFiles.[rnd.Next(0,allFiles.Count()-1)]

