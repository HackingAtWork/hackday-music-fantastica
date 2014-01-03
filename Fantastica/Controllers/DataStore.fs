module DataStore

open Fantastica.Api
open Fantastica.Api.Entities

let saveUser (u : User) =
        RavenAccess.Instance.Save u

let getAllUsers = 
        RavenAccess.Instance.Query<User>()