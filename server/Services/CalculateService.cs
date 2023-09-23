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

            await responseStream.WriteAsync(new CalculateResponse{Total = result});
        }
    }
}
