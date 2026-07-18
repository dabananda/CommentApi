using FluentValidation;

namespace CommentApi.Common.Abstraction
{
    public class Sender(IServiceProvider provider) : ISender
    {
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();

            var validatorType = typeof(IValidator<>).MakeGenericType(requestType);
            if (provider.GetService(validatorType) is IValidator validator)
            {
                var context = new ValidationContext<object>(request);
                var validationResult = await validator.ValidateAsync(context, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BuildValidationFailure<TResponse>(errors);
                }
            }

            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var handler = provider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod("Handle") ?? throw new InvalidOperationException("Handler method not found.");

            return await (Task<TResponse>)method.Invoke(handler, [request, cancellationToken])!;
        }

        private static TResponse BuildValidationFailure<TResponse>(List<string> errors)
        {
            var failureMethod = typeof(TResponse).GetMethod(
                "Failure",
                [typeof(string), typeof(List<string>), typeof(ExceptionType)])
                ?? throw new InvalidOperationException($"{typeof(TResponse).Name} has no compatible Failure method.");

            var result = failureMethod.Invoke(null, ["Validation failed", errors, ExceptionType.Validation]);
            return (TResponse)result!;
        }
    }
}