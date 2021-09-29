using Amazon.SQS;
using Amazon.SQS.Model;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.AWSService.SQSHandlers
{
    public class SQSPublisher<T> : ISQSPublisher<T> where T : class
    {

        #region Service

        /// <summary>
        /// The SQS
        /// </summary>
        private readonly IAmazonSQS _SQS;

        /// <summary>
        /// The logger service
        /// </summary>
        private readonly ILoggerService _loggerService;

        #endregion

        #region Queue Url

        /// <summary>
        /// The queue URL
        /// </summary>
        private readonly string _queueUrl;


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SQSPublisher{T}"/> class.
        /// </summary>
        /// <param name="SQS">The SQS.</param>
        /// <param name="queueUrl">The queue URL.</param>
        public SQSPublisher(IAmazonSQS SQS, string queueUrl, ILoggerService loggerService)
        {
            _SQS = SQS;
            _queueUrl = queueUrl;
            _loggerService = loggerService;
        }

        #endregion

        #region Send Message Queue  Async

        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task SendMessageAsync(T model)
        {
            try
            {
                var message = model.ToJson();
                var sendMessageRequest = new SendMessageRequest()
                {
                    QueueUrl = _queueUrl,
                    MessageBody = message,
                    MessageGroupId = typeof(T).Name,
                    MessageDeduplicationId = Guid.NewGuid().ToString()
                };
                // Post message or payload to queue  
                var sendResult = await _SQS.SendMessageAsync(sendMessageRequest);
                if (sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _loggerService.LogInfo("Send Message Sucessfully");

                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "");
            }
        }

        #endregion

        #region Send Mutiple Message Queues  Async

        /// <summary>
        /// Sends the messages asynchronous.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public async Task SendMessagesAsync(List<T> messages)
        {
            try
            {
                foreach (var item in messages)
                {
                    await SendMessageAsync(item);
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "");

            }

        }

        #endregion

        #region Delete Message Async

        /// <summary>
        /// Deletes the message asynchronous.
        /// </summary>
        /// <param name="messageReceiptHandle">The message receipt handle.</param>
        public async Task DeleteMessageAsync(string messageReceiptHandle)
        {
            try
            {
                //Deletes the specified message from the specified queue  
                var deleteResult = await _SQS.DeleteMessageAsync(_queueUrl, messageReceiptHandle);
                if (deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _loggerService.LogInfo("Delete Message Sucessfully");
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "");
            }
        }

        #endregion


    }

    public class SQSSubscriber<T> : ISQSSubscriber<T> where T : class
    {
        #region Service

        /// <summary>
        /// The SQS
        /// </summary>
        private readonly IAmazonSQS _SQS;

        /// <summary>
        /// The logger service
        /// </summary>
        private readonly ILoggerService _loggerService;

        #endregion

        #region Queue Url

        /// <summary>
        /// The queue URL
        /// </summary>
        private readonly string _queueUrl;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SQSSubscriber{T}"/> class.
        /// </summary>
        /// <param name="SQS">The SQS.</param>
        /// <param name="queueUrl">The queue URL.</param>
        public SQSSubscriber(IAmazonSQS SQS, string queueUrl , ILoggerService loggerService)
        {
            _SQS = SQS;
            _queueUrl = queueUrl;
            _loggerService = loggerService;
        }

        #endregion

        #region Process Queue Async

        /// <summary>
        /// Receives the message asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> ReceiveMessageAsync()
        {
            try
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = AppSettingValues.MaxNumberOfMessages,
                    WaitTimeSeconds = AppSettingValues.WaitTimeSeconds,
                };

                var result = await _SQS.ReceiveMessageAsync(request);
                var queues = result.Messages;

                if (!queues.Any())
                {
                    return CollectionConstants.Empty<T>();
                }

                var data = queues.Select(x => x.Body.ToMapperObject<T>()).ToList();
                foreach (var item in queues)
                {
                    await DeleteMessageAsync(item.ReceiptHandle);
                }

                return data;
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Error when fetching");
                return CollectionConstants.Empty<T>();
            }
        }

        #endregion

        #region Delete Message Async

        /// <summary>
        /// Deletes the message asynchronous.
        /// </summary>
        /// <param name="messageReceiptHandle">The message receipt handle.</param>
        public async Task DeleteMessageAsync(string messageReceiptHandle)
        {
            try
            {
                //Deletes the specified message from the specified queue  
                var deleteResult = await _SQS.DeleteMessageAsync(_queueUrl, messageReceiptHandle);
                if (deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _loggerService.LogInfo("Message Queue was done sucessfully");
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex, "Delete Message UnSucessfully");
            }
        }

        #endregion
    }
}
