## Ventrian Property Agent ##

**Property Agent** for DotNetNuke is a templated property module that allows you to manage and display all kinds of properties from cars to boats to real estate. 

### Sorce Code Installation Steps ###

To use source file and debug it, follow this steps:

- Install Property Agent Modules on your DNN 7.4.2+ installation using the PA installation package
- Copy Github PA solution under Desktopmodules like PropertyAgentSourceCode 
- Go in Host->Extension and edit all PA modules definitions to point to Source Code folder instead PA installation folders
- Add alias for www.ventrian.com on your DNN Website with Admin Site Configuration
- Add binding www.ventrian.com on your IIS for your website
- Edit host file and add this: **127.0.0.1 www.ventrian.com**
- Close and reopen your browser and test it opening www.ventrian.com
- Open Visual Studio with administrator privileges
- Open **Ventrian.PropertyAgent.sln** and lunch in debug mode
