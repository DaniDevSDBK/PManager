using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PManager.Model
{
    public interface IMessenger
    {
        void Register<TMessage>(object recipient, Action<TMessage> action);
        void Unregister<TMessage>(object recipient);
        void Send<TMessage>(TMessage message);
    }
}
