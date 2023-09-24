using System.Threading.Tasks;
using System.Collections.Generic;
using client;
using calculate;
using Grpc.Net.Client;

Console.WriteLine("App is starting");

//***********Unary
// using var channel = GrpcChannel.ForAddress("http://localhost:5292");
// var client = new Greeter.GreeterClient(channel);

// var reply = await client.SayHelloAsync(new HelloRequest { Name = "Taylan" });
// Console.WriteLine("Greeting: " + reply.Message);

// var response = await client.AddAsync(new AddRequest{Sayi1 = 12 , Sayi2 = 22});
// Console.WriteLine($"Toplam: {response.Toplam}");



//***********Server Streaming
// using var channel = GrpcChannel.ForAddress("http://localhost:5292");
// var client = new Calculate.CalculateClient(channel);

// var response = client.CalculateAll(new CalculateRequest { Number1 = 10, Number2 = 2 });

// var ct = new CancellationTokenSource();
// while (await response.ResponseStream.MoveNext(ct.Token))
// {
//     Console.WriteLine(response.ResponseStream.Current.Total);
// }

//***********Client Streaming
using var channel = GrpcChannel.ForAddress("http://localhost:5292");
var client = new Calculate.CalculateClient(channel);

var names = new List<string>{"Ali","Ayşe","Metin","Eren"};

var clientStreamingCall = client.CombineName();
foreach (var name in names)
{
    await Task.Delay(1000);
    await clientStreamingCall.RequestStream.WriteAsync(new CombineNameRequest { Name = name });
}

await clientStreamingCall.RequestStream.CompleteAsync();

var response = await clientStreamingCall.ResponseAsync;

Console.WriteLine($"Response: {response.CombinedName}");


Console.WriteLine("Press any key to exit...");
Console.ReadKey();
