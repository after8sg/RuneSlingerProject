
using RuneSlinger.Base.Abstract;
namespace RuneSlinger.server.Abstract
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Handle(CommandContext context, TCommand command);
    }
}
