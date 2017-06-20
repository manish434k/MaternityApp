Please find attached the source for the Maternity App Milestone 1. To build, simply open the solution in Visual Studio 2015, restore the nuget packages (right click on the solution node in the solution explorer and select “Restore nuget Packages”) and then build.
 
Note that in order to run the app in debug after building, you will need locally installed configuration and sample data files. Copy those from the source as follows:
 
<sourcefolder>\SharedResources\ConfigData\ProgramData.zip -> extract to \programdata
<sourcefolder>\SharedResources\SampleData\UserAppDataRoaming.zip -> extract to \users\<yourusername>\appdata\roaming