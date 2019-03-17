# Mallos.Networking
High-level networking

> This project is still a work in progress, a lot of things are not implemented yet.

## Features

- [x] Chat system

#### Create Server
```cs
var server = new NetServer(serviceProvider);

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
```
