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
// using var channel = GrpcChannel.ForAddress("http://localhost:5292");
// var client = new Calculate.CalculateClient(channel);

// var names = new List<string>{"Ali","Ayşe","Metin","Eren"};

// var clientStreamingCall = client.CombineName();
// foreach (var name in names)
// {
//     await Task.Delay(1000);
//     await clientStreamingCall.RequestStream.WriteAsync(new CombineNameRequest { Name = name });
// }

// await clientStreamingCall.RequestStream.CompleteAsync();

// var response = await clientStreamingCall.ResponseAsync;

// Console.WriteLine($"Response: {response.CombinedName}");



//***********bi-directional
 using var channel = GrpcChannel.ForAddress("http://localhost:5292");
 var client = new Calculate.CalculateClient(channel);

var studentList = new List<Student>();
var s1 = new Student();
s1.Name = "Taylan";
s1.Notes.Add(new List<int> { 12, 34, 56 });
var s2 = new Student();
s2.Name = "Veli";
s2.Notes.Add(new List<int> { 45, 34, 56 });
var s3 = new Student();
s3.Name = "Deli";
s3.Notes.Add(new List<int> { 20, 34, 15 });
studentList.Add(s1);
studentList.Add(s2);
studentList.Add(s3);

var clientSteamingCall = client.TotalAllArray();

var t1 = Task.Run(async () =>
{
    foreach (var student in studentList)
    {
        await Task.Delay(1500);
        Console.WriteLine($"Student Sending --> {student.Name}");
        clientSteamingCall.RequestStream.WriteAsync(new TotalAllArrayRequest { Student = student });
    }
});

var ct = new CancellationTokenSource();

while (await clientSteamingCall.ResponseStream.MoveNext(ct.Token))
{
    Console.WriteLine($"{clientSteamingCall.ResponseStream.Current.Message}");
}

await t1;
await clientSteamingCall.RequestStream.CompleteAsync();


Console.WriteLine("Press any key to exit...");
Console.ReadKey();
