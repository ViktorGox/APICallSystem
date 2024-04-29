Project contains C# code which allows you to represent API system within a system and allow execution of requests to it.

**Still work in progress**

To represent an API:
Requires context class instance. - It represents a single entire API.
To create context class instance, `IHttpReqResponseAdapter` and `IHttpReqBodyAdapter` are required. 

`IHttpReqResponseAdapter` is used to adapt the received data from the back end into whatever is desired in the current project. JSON, decryption and other are possible through this.

`IHttpReqBodyAdapter` is used to adapt the data which will be sent to the back end. Again, perhaps convert to JSON or encryption.

The context class then must be seeded with the end point entities. Create simple model classes with variables representing the back end version of the received data. To give body to the request, an instance of it with the populated variables is required, so one or more constructors could be necessary as well.

Add the entity to the context using `.AddEntity(Type, string)` method.

To make requests to the API, use .Get\<T>() method. The T will show the method which entity you are looking to fetch.

Simple example code

```
            JSONHttpReqResponseAdapter jsonResponseAdapter = new();
            JSONHttpReqBodyAdapter jsonBodyAdapter = new();

            Context context = new(UrlFactory.Create(true, "localhost", 7225, "api"), jsonResponseAdapter, jsonBodyAdapter);

            context.AddEntity(typeof(Message), "Module"); 
            context.AddEntity(typeof(User), "Test");

            User dummyUser = new User("112", "Obvious name", "@av");

            context.Get<User>()?.Get(new Guid("62b89e37-0a89-4233-5db9-08dc4dcaf70c"), OnSuccess, OnFailure);
            context.Get<User>()?.Post(dummyUser, OnSuccess, OnFailure);
```

TO ADD: How entity works, URLFactory class. the OnSuccess, OnFailure methods.