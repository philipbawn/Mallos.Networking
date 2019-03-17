# Mallos.Networking
High-level networking

> This project is still a work in progress, a lot of things are not implemented yet.

### Features

- [ ] User Managment
    - [ ] Authentication
    - [ ] Permissions logic
- [ ] Chat system
    - [ ] Messages (Global, Channel, DM)
    - [ ] Server Commands
- [ ] Game Session
    - [ ] Sync
    - [ ] Live Updates

### Sample

#### Create Server
```cs
var userStorage = new InMemoryUserStorage<IdentityUser>();
var userManager = new UserManager<IdentityUser>(userStorage);

var server = new NetServer<IdentityUser>(serviceProvider, userManager);

server.Chat.Received += (message) =>
{
    Console.WriteLine(message.ToString());
};

server.Start();
```

#### Create Client
```cs
var client = new NetClient(serviceProvider);

client.Chat.Received += (message) =>
{
    Console.WriteLine(message.ToString());
};

client.Start(new NetConnectionParameters("eric", "password", "localhost"));

client.Chat.SendMessage("Hello World");
```
