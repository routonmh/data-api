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
 
## Controllers
Controllers are methods bound to route handlers along with
their parameters. Parameters in these methods can have
annotations to indicate that the value comes from 
the request body, header, or query parameters.

### Controller Example:

## Models
Models are mainly used to interact with the database and issue queries.

## Middleware
Middleware is applied to requests **before** they reach the
controller methods.
Common usages of middleware are to check for authentication and
modify headers. Another good usage is to log request traffic & usage.
Multiple middlewares that call a `next` method are frequently used to
run several processes before handling the request. If a middleware class'
`Invoke` method is not called, the request is short-circuited and the controller
never receives the full request. It is up to the middleware that terminates the
request to set an appropriate status code and return a meaningful error message.

### Middleware Example
The `Invoke(HttpContext)` method is given to any class to be used as middleware.
When the middleware is called this method takes the `HttpContext` which includes
the request and response (to be sent) data. Any middleware's `Invoke` method
can be used to affect the response **after** the controller method has
been called in the part of the method below the call to `next(context)`.

```
 public async Task Invoke(HttpContext context)
        {
            Guid sessionID = Guid.Parse(context.Request.Headers[HeaderFields.SESSION_ID_HEADER_NAME]);

            logger.LogInformation("Incrementing request count on session id: " + sessionID.ToString());

            // No need to await the increment log query.
            UserSessionsModel.IncrementSessionRequestCount(sessionID);
            UserSessionsModel.UpdateSessionLastRequestTime(sessionID);

            await next(context);
            
            // Any code here is meant to affect the response
        }
```

## Authentication

# Documentation

### Code (Javadoc)

### REST API

# Testing

An awesome feature of testing in ASP.NET Core is the ability to
test route handlers and their corresponding controller methods in a
single call. This may not seem like a significant feature but after
working with testing route and controllers in Node.js this is a huge
convienience.

