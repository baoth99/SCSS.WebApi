using System.Collections.Generic;


namespace SCSS.QueueEngine.QueueRepositories
{
    public interface IQueueRepository<T> where T : class
    {
        /// <summary>
        /// Pushes the queue.
        /// </summary>
        /// <param name="model">The model.</param>
        void PushQueue(T model);

        /// <summary>
        /// Dequeues the queue.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        T DequeueQueue(T model);

        /// <summary>
        /// Peeks the queue.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        T PeekQueue(T model);

        /// <summary>
        /// Consumes the queue.
        /// </summary>
        /// <returns></returns>
        IList<T> GetAllQueue();

        /// <summary>
        /// Consumes the dequeue queue.
        /// </summary>
        /// <returns></returns>
        IList<T> ConsumeDequeueQueue(int length);

        /// <summary>
        /// Queues the length.
        /// </summary>
        /// <returns></returns>
        int QueueLength();
    }
}
