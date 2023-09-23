
using client;
using Grpc.Net.Client;

Console.WriteLine("App is starting");

using var channel = GrpcChannel.ForAddress("http://localhost:5292");
var client = new Greeter.GreeterClient(channel);

// var reply = await client.SayHelloAsync(new HelloRequest { Name = "Taylan" });
// Console.WriteLine("Greeting: " + reply.Message);

var response = await client.AddAsync(new AddRequest{Sayi1 = 12 , Sayi2 = 22});
Console.WriteLine($"Toplam: {response.Toplam}");


Console.WriteLine("Press any key to exit...");
Console.ReadKey();
