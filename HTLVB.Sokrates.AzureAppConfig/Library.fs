module Sokrates.AzureAppConfig

open Azure.Identity
open Microsoft.Extensions.Configuration
open System

type SokratesConfig =
    {
        ``web-service-url``: string
        ``user-name``: string
        password: string
        ``school-id``: string
        ``client-certificate``: string
    } with
    member x.Build() : Sokrates.Config = {
        WebServiceUrl = Uri(x.``web-service-url``)
        UserName = x.``user-name``
        Password = x.password
        SchoolId = x.``school-id``
        ClientCertificate = Convert.FromBase64String(x.``client-certificate``)
    }

let getConfig (appConfigurationEndpoint: Uri) = 
    let config =
        ConfigurationBuilder()
            .AddAzureAppConfiguration(
                (
                    fun config ->
                        config.Connect(
                            appConfigurationEndpoint,
                            DefaultAzureCredential()
                        )
                        |> ignore
                        config.ConfigureKeyVault(fun keyVault ->
                            keyVault.SetCredential(DefaultAzureCredential()) |> ignore
                        )
                        |> ignore
                ),
                optional = false
            )
            .Build()
    ConfigurationBinder.Get<SokratesConfig>(config.GetSection("sokrates")).Build()
