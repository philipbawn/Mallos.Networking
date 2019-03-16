# Mallos.Networking
High-level networking

> This project is still a work in progress, a lot of things are not implemented yet.

#### Create Server
```cs
var server = new NetServer(serviceProvider);
```

#### Create Client
```cs
var client = new NetClient(serviceProvider, new NetConnectionParameters("", "", "127.0.0.1"));
```
