using System;

namespace console_service.Patterns.Command
{
    public interface ICommand
    {
        Action<object> Action { get; set; }
        Func<object, Object> GetAction { get; set; }

        void Execute(object param);

        object Call(object param);

    }
}