using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.AWSService.SQSHandlers
{
    public interface ISQSPublisher<T> where T : class
    {
        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task SendMessageAsync(T model);

        /// <summary>
        /// Deletes the message asynchronous.
        /// </summary>
        /// <param name="messageReceiptHandle">The message receipt handle.</param>
        /// <returns></returns>
        Task DeleteMessageAsync(string messageReceiptHandle);

        /// <summary>
        /// Sends the messages asynchronous.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        Task SendMessagesAsync(List<T> messages);
    }


    public interface ISQSSubscriber<T> where T : class
    {
        /// <summary>
        /// Receives the message asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<T>> ReceiveMessageAsync();

        /// <summary>
        /// Deletes the message asynchronous.
        /// </summary>
        /// <param name="messageReceiptHandle">The message receipt handle.</param>
        /// <returns></returns>
        Task DeleteMessageAsync(string messageReceiptHandle);
    }
}
