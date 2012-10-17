// -----------------------------------------------------------------------
// <copyright file="ITaskItemExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.MSBuild
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ITaskItemExtensions
    {
        public static string[] ToStringArray(this ITaskItem[] taskitems)
        {
            var result = new List<string>();
            if (taskitems != null)
            {
                foreach (ITaskItem item in taskitems)
                {
                    result.Add(item.ItemSpec);
                }
            }
            return result.ToArray();
        }
    }
}
