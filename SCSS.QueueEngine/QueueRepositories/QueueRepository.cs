using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace SCSS.QueueEngine.QueueRepositories
{
    public class QueueRepository<T> : IQueueRepository<T> where T : class
    {
        #region Concurrent Queue

        /// <summary>
        /// The queue
        /// </summary>
        private readonly ConcurrentQueue<T> _queue;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueRepository{T}"/> class.
        /// </summary>
        /// <param name="queue">The queue.</param>
        public QueueRepository(ConcurrentQueue<T> queue)
        {
            _queue = queue;
        }

        #endregion

        #region Peek Queue

        /// <summary>
        /// Peeks the queue.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public T PeekQueue(T model)
        {
            _queue.TryPeek(out T data);
            return data;
        }

        #endregion

        #region Get All Queue

        /// <summary>
        /// Consumes the queue.
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAllQueue()
        {
            return _queue.ToList();
        }

        #endregion

        #region Push Queue

        /// <summary>
        /// Pushes the queue.
        /// </summary>
        /// <param name="model">The model.</param>
        public void PushQueue(T model)
        {
            _queue.Enqueue(model);
        }

        #endregion

        #region Dequeue Queue

        /// <summary>
        /// Dequeues the queue.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public T DequeueQueue(T model)
        {
            _queue.TryDequeue(out T data);
            return data;
        }

        #endregion

        #region Consume DequeueQueue

        /// <summary>
        /// Consumes the dequeue queue.
        /// </summary>
        /// <returns></returns>
        public IList<T> ConsumeDequeueQueue(int length)
        {
            List<T> result = new List<T>();

            if (!_queue.Any())
            {
                return result;
            }

            for (int i = 1; i <= length; i++)
            {
                _queue.TryDequeue(out T data);
                if (data != null)
                {
                    result.Add(data);
                }
            }

            return result;
        }

        #endregion

        #region Queue Length

        /// <summary>
        /// Queues the length.
        /// </summary>
        /// <returns></returns>
        public int QueueLength()
        {
            return _queue.Count;
        }

        #endregion
    }
}
