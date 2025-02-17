param(
    [Parameter(Mandatory)]
    [string] $lambdaName
)

# This script assumes you are running it at the root of:
# `C:\src\github.com\ongzhixian\EimaAmazonWebServices`
# Example call:
# .\New-Lambda.ps1 ConfigurationLambdas

# Lambda projects naming convention
# <ObjectType>Lambdas

dotnet new lambda.EmptyFunction -n $lambdaName

# This command creates 2 projects at the following location (which is not how we want it):
# C:\src\github.com\ongzhixian\EimaAmazonWebServices\ProjectLambdas\src\ProjectLambdas
# C:\src\github.com\ongzhixian\EimaAmazonWebServices\ProjectLambdas\test\ProjectLambdas.Tests

# We want to move them to `src` directory instead
Move-Item .\$lambdaName\src\$lambdaName\ .\src\
Move-Item .\$lambdaName\test\$lambdaName.Tests\ .\src\

# Remove the now empty folders
Remove-Item -Recurse .\$lambdaName\

# The add those projects to solution
dotnet sln .\src\EimaAws.sln add .\src\$lambdaName\ --solution-folder LambdaFunctions
dotnet sln .\src\EimaAws.sln add .\src\$lambdaName.Tests\ --solution-folder LambdaFunctions

Write-Host All done $lambdaName