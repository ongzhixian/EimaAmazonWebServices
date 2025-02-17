Write-Host "Publishing dotnet assets"

dotnet publish .\src\HelloLambda\ --configuration Release --runtime linux-x64 --output .\src\HelloLambda\publish
dotnet publish .\src\ProjectLambdas\ --configuration Release --runtime linux-x64 --output .\src\ProjectLambdas\publish

Write-Host "Deploying EimaAwsStack"

cdk deploy

# Use `cdk destroy` to remove