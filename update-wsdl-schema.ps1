$ProgressPreference = "SilentlyContinue"

$ClientCertificatePath = "sokrates.pfx"

$Base64Content = az appconfig kv list -n ac-htl-utils --key sokrates:client-certificate --resolve-keyvault true --query "[].value" -o tsv
$ContentBytes = [Convert]::FromBase64String($Base64Content)
[IO.File]::WriteAllBytes($ClientCertificatePath, $ContentBytes)

$SchemaUri = "https://wwwws.sokrates-bund.at/SOKBWS/ws/dataexchange?xsd=1"
$ClientCertificate = Get-PfxCertificate $ClientCertificatePath
Invoke-WebRequest $SchemaUri -Certificate $ClientCertificate -OutFile HTLVB.Sokrates\sokrates.xsd -UseBasicParsing

Remove-Item $ClientCertificatePath
