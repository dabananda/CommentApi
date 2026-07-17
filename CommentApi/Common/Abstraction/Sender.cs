namespace CommentApi.Common.Abstraction
{
    public class Sender(IServiceProvider provider) : ISender
    {
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var handler = provider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod("Handle") ?? throw new InvalidOperationException("Handler method not found.");
            
            return await (Task<TResponse>)method.Invoke(handler, [request, cancellationToken])!;
        }
    }
}