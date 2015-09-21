# IIS Manager Troubleshooting



### Error message
```
An exception of type 'System.UnauthorizedAccessException' occurred in Microsoft.Web.Administration.dll but was not handled in user code

Additional information: Filename: redirection.config

Error: Cannot read configuration file due to insufficient permissions
```

### Solution
* Make sure your running the as an administrator. 
* Running the test from within Visual Studio Test Explorer will not work



### Error message
```
Microsoft.Web.Administration.ServerManagerException : Application pools cannot be started unless the Windows Activation Service (WAS) is running.
```

### Solution
* Check IIS is installed on the machine your trying to manage



### Error message
```
Retrieving the COM class factory for remote component with CLSID {2B72133B-3F5B-4602-8952-803546CE3344} from machine failed due to the following error: 800706ba
```

### Solution
* Disable windows firewall