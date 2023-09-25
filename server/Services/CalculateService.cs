using Grpc.Core;
using server;

namespace calculate.Services;

public class CalculateService : Calculate.CalculateBase
{
    public override async Task CalculateAll(CalculateRequest request, IServerStreamWriter<CalculateResponse> responseStream, ServerCallContext context)
    {
        var process = new char[] { '+', '-', 'x', '/' };
        string result = "";
        foreach (var p in process)
        {
            await Task.Delay(2000);

            switch (p)
            {
                case '+':
                    result = $"Toplam: {request.Number1 + request.Number2}";
                    break;
                case '-':
                    result = $"Fark: {request.Number1 - request.Number2}";
                    break;
                case 'x':
                    result = $"Çarpım: {request.Number1 * request.Number2}";
                    break;
                case '/':
                    result = $"Bölme: {request.Number1 / request.Number2}";
                    break;
            }

            await responseStream.WriteAsync(new CalculateResponse { Total = result });
        }
    }

    public override async Task<CombineNameResponse> CombineName(IAsyncStreamReader<CombineNameRequest> requestStream, ServerCallContext context)
    {
        var names = new List<string>();

        while (await requestStream.MoveNext(context.CancellationToken))
        {
            await Task.Delay(1000);
            Console.WriteLine($"Name are receiving!! Name: {requestStream.Current.Name}");
            names.Add(requestStream.Current.Name);
        }

        Console.WriteLine($"Response is sending...");
        return new CombineNameResponse
        {
            CombinedName = string.Join(';', names.ToArray())
        };
    }

    public override async Task TotalAllArray(IAsyncStreamReader<TotalAllArrayRequest> requestStream, IServerStreamWriter<TotalAllArrayResponse> responseStream, ServerCallContext context)
    {
        var t1 = Task.Run(async () =>
        {
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                var student = requestStream.Current.Student;
                Console.WriteLine($"Receive a student --> {student.Name}");
                var totalNote = student.Notes.Aggregate((temp, x) => temp + x);

                await Task.Delay(1000);

                await responseStream.WriteAsync(new TotalAllArrayResponse { Message = $"{student.Name}-{totalNote}" });
            }
        });

        await t1;
    }
}
