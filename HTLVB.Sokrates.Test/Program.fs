module Sokrates.Test.Program

open Expecto
open Sokrates
open System

let private sokratesConfig = AzureAppConfig.getConfig (Uri "https://ac-htl-utils.azconfig.io")
let private sokratesApi = SokratesApi(sokratesConfig)

let tests =
    testList "Fetch students" [
        testCaseAsync "Can fetch student gender" <| async {
            let! students = sokratesApi.FetchStudents()
            students
            |> List.iter (fun v -> printfn $"%s{v.LastName} %s{v.FirstName1} (%s{v.SchoolClass}): %O{v.Gender}")
        }
    ]

[<EntryPoint>]
let main args =
    runTestsWithCLIArgs [] args tests
