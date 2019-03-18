# Mallos.Networking
High-level networking

> This project is still a work in progress, a lot of things are not implemented yet.

### Features

- [x] User Managment
- [x] User Authentication (Login)
- [ ] User Authentication Providers (Steam etc)
- [ ] User Permissions logic (Admin etc)
- [ ] Chat system
	- [ ] Messages (Global, Channel, DM)
	- [ ] Server Commands
- [ ] Game Session
    - [ ] Sync
    - [ ] Live Updates

### Sample

#### Create Server
```cs
public async Task CreateServer()
{
	var userStorage = new InMemoryUserStorage<IdentityUser>();
	var userManager = new UserManager<IdentityUser>(userStorage);

	await userManager.CreateAsync(new IdentityUser("Eric"), "password");

	var server = new NetServer<IdentityUser>(serviceProvider, userManager);

	server.Chat.Received += (message) =>
	{
		Console.WriteLine(message.ToString());
	};

	await server.Start();
}
```

#### Create Client
```cs
public async Task CreateClient()
{
	var client = new NetClient(serviceProvider);

	client.Chat.Received += (message) =>
	{
		Console.WriteLine(message.ToString());
	};

	await client.Start(new NetConnectionParameters("eric", "password", "localhost"));

	client.Chat.SendMessage("Hello World");
}
```
