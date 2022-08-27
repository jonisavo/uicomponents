using System.Collections;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace UIComponents.Tests.Utilities
{
    public static class TaskExtensions
    {
        public static IEnumerator AsEnumerator(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                ExceptionDispatchInfo.Capture(task.Exception).Throw();
            }

            yield return null;
        }
    }
}
