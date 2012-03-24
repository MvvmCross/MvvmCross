param($installPath, $toolsPath, $package, $project)

ForEach ($projectItem in $project.ProjectItems) 
{ 
    if ($projectItem.Name -eq "Toolkit.Content") 
    {
        ForEach ($resourceItem in $projectItem.ProjectItems) 
        {
            if (($resourceItem.Name -eq "ApplicationBar.Cancel.png") -Or ($resourceItem.Name -eq "ApplicationBar.Check.png") -Or ($resourceItem.Name -eq "ApplicationBar.Delete.png") -Or ($resourceItem.Name -eq "ApplicationBar.Select.png"))
            {
                $resourceItem.Properties.Item("ItemType").Value = "Content";
            }
        }
    } 
}