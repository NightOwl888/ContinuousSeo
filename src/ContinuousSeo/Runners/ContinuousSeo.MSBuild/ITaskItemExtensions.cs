#region Copyright
// -----------------------------------------------------------------------
//
// Copyright (c) 2012, Shad Storhaug <shad@shadstorhaug.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// -----------------------------------------------------------------------
#endregion

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
