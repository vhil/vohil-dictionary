using System.Threading.Tasks;
using Sitecore.Framework.Messaging;

namespace Pintle.Dictionary.Messaging
{
	public class DictionaryItemMessageHandler : IMessageHandler<DictionaryItemMessage>
	{
		public Task Handle(
			DictionaryItemMessage message, 
			IMessageReceiveContext receiveContext, 
			IMessageReplyContext replyContext)
		{
			throw new System.NotImplementedException();
		}
	}
}
