#Based on https://docs.microsoft.com/en-us/azure/app-service/scripts/app-service-powershell-continuous-deployment-github

Write-Host "This script will create a new free Azure web site with continuos deployment from github repo, Azure PowerShell is required to run this"

Connect-AzureRmAccount

$gitrepo = Read-Host -Prompt 'Your git repo url'
$gittoken = Read-Host -Prompt 'Your git token here (with repo access)'
$resourcegroupname = Read-Host -Prompt 'Resource group name'
$webappname = Read-Host -Prompt 'Web app name (used for service plan as well)'
$location="West Europe"

# Create a resource group.
New-AzureRmResourceGroup -Name $resourcegroupname -Location $location

# Create an App Service plan in Free tier.
New-AzureRmAppServicePlan -Name $webappname -Location $location -ResourceGroupName $resourcegroupname -Tier Free

# Create a web app.
New-AzureRmWebApp -Name $webappname -Location $location -AppServicePlan $webappname -ResourceGroupName $resourcegroupname

# SET GitHub
$PropertiesObject = @{
	token = $gittoken;
}
Set-AzureRmResource -PropertyObject $PropertiesObject -ResourceId /providers/Microsoft.Web/sourcecontrols/GitHub -ApiVersion 2015-08-01 -Force

# Configure GitHub deployment from your GitHub repo and deploy once.
$PropertiesObject = @{
	repoUrl = "$gitrepo";
	branch = "master";
}
Set-AzureRmResource -PropertyObject $PropertiesObject -ResourceGroupName $resourcegroupname -ResourceType Microsoft.Web/sites/sourcecontrols -ResourceName $webappname/web -ApiVersion 2015-08-01 -Force