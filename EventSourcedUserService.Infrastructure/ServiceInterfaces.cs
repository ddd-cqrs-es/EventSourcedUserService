namespace UserService.Infrastructure
{
    public interface ICommandHandler<TCommand>
    {
        void HandleCommand(TCommand itemCommand);
    }

    public interface Handles<TCommand>
    {
        void Handle(TCommand command);
    }
}
