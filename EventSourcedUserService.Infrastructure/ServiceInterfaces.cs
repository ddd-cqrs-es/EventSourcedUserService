namespace UserService.Infrastructure
{
    public interface ICommandHandler<TCommand>
    {
        void HandleCommand(TCommand command);
    }

    public interface Handles<TCommand>
    {
        void Handle(TCommand command);
    }
}
