<p align="center"> 
<img src="https://cdn2.hubspot.net/hubfs/498921/cloudwatch.png">
</p>

**AWS CloudWatch Monitor**

This utility allows you to Push Used Memory Stats of your Windows Instances to AWS CloudWatch without the need of configuring any JSON files or installing AWS SSM Agent.
The way it works is by utilising AWS SDK kit and ComputerInfo Class from .NET.  Once the application is installed it will run in the background as a service and all you need is your AWS AccessKey and SecretKey.

 <font color=red>**Coming Soon**</span>
 
#### <a href="url"><img src="http://icons.iconarchive.com/icons/dtafalonso/yosemite-flat/32/Folder-icon.png" align="left" height="30" width="30" ></a> Files and Folders

| Name | Description | Last Update |
| --- | --- | --- |
| AWS CloudWatch | Contains CloudWatch Source Code | 7th of November 2017   |
| AWS Input | Contains AWS Config Source Code | 7th of November 2017  |
| Installer | Contains the Source Code for the installer | 7th of November 2017  |
| Package | Contains the compiled version and ready to use | 7th of November 2017  |

#### <a href="url"><img src="https://www.cuttingedgecreations.com/var/theme/images/instructions.png" align="left" height="30" width="30" ></a>Instructions

 1. Download the packaged version
 2. Run the setup.exe and then click Install
 3. You will be promoted to enter your AWS AccessKey and AWS SecretKey

#### <a href="url"><img src="http://ipfe.co/filefoto/user-permissions.png" align="left" height="30" width="30" ></a> Permissions Required

 - Microsoft .NET 4
 - Microsoft Windows Server 2008 R2 or later
 - Either use IAM role to provide CloudWatch Access or provide a specific user AccessKey and SecretKey to submit data to CloudWatch 

#### <a href="url"><img src="http://www.free-icons-download.net/images/file-icon-28038.png" align="left" height="30" width="30" ></a> How does it work?

Once the application is installed it will install a service which will run in the background and push Used Memory stats to your CloudWatch account. 
During the installation you will see a new Winodws will appear on the screen to ask you for the AWS AccesKey and AWS SecretKey. This will help configurator the service for you. 

![AWS_Configurator](http://img.support.vmturbo.com/AWS/AWSInput.png)
