
using Grpc.Net.Client;
using server;

Console.WriteLine("App is starting");

using var channel = GrpcChannel.ForAddress("http://localhost:5292");
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "Taylan" });
Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
