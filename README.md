# data-api

**Template .NET Core Web API**

This is a .NET Core 2.1 LTS project. 2.1 is currently the 
highest version supported by Google App Engine. (3 is the latest)

# Setup

### Environment Variables

* `JWT_SECRET=AnySecretStringGoesHere` 
    * **Used to sign and verify JWT Tokens** - Make up any string for a secret. 
    This is so we can trust the data in the tokens we read.
* `LOCAL_DB_CONNECTION=Data Source=localhost;Initial Catalog=datapidb;User ID=serveruser;Password=pass`
    * **DB Connection String** - Some insecure example user credentials
    
### MySQL Setup 
There is an included sample database schema along with some records.
* Run the `/db/datapidb.sql` file on a local mysql instance. 
* Credentials for the sample database from the file are
    * **user**: serveruser
    * **password**: pass

# Building the Project 

### CLI

* Navigate to the application directory
    * `cd ./data-api`

* Create the build 
    * `dotnet publish -c Release`

* Navigate to where the executable was built:
    * `cd /bin/Release/netcoreapp2.1/publish/`
    
* Run it. The port will be determined by the `"applicationUrl"` value
 in `/data-api/Properties/launchSettings.json`
    * `dotnet data-api.dll`   
    
# How it Works 

Quick information to get you started with the
 structure of MVC .NET Core: 
 
### Controllers
### Models 
### Middleware
### Authentication
