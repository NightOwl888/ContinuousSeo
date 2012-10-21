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

namespace ContinuousSeo.Core.Announcers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class Announcer : IAnnouncer
    {
        public virtual void Header(string toolName)
        {
            WriteLine(toolName, true);
        }
        
        public virtual void Heading(string message)
        {
            WriteLine(message, true);
        }

        public virtual void Say(string message)
        {
            WriteLine(message, true);
        }

        public virtual void ElapsedTime(TimeSpan timeSpan)
        {
            WriteLine(string.Format("=> {0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds), true);
        }

        public virtual void Error(string message)
        {
            WriteLine(string.Format("!!! {0}", message), true);
        }

        public abstract void Write(string message, bool escaped);
        public abstract void WriteLine(string message, bool escaped);

    }
}
