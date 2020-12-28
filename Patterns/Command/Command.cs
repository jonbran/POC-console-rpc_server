using System;

namespace console_service.Patterns.Command
{
    public class Command : ICommand
    {
        private Action<Object> action;
        public Action<Object> Action
        {
            get { return action; }
            set { action = value; }
        }

        private Func<object, Object> getAction;

        public Func<object, Object> GetAction
        {
            get { return getAction; }
            set { getAction = value; }
        }



        public Command(Action<Object> action, Func<object, object> func) {
            this.Action = action;
            this.GetAction = func;
        }

        public void Execute(object param)
        {
            this.action.Invoke(param);
        }

        public object Call(object param)
        {
            if (this.GetAction != null)
            {
                return this.getAction.Invoke(param);
            }

            throw new NotImplementedException("GetAction not set");
        }
    }
}