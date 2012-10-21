﻿#region Copyright
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

namespace ContinuousSeo.Core.IO
{
    using System;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StreamFactory : IStreamFactory
    {
        #region IStreamFactory Members

        public System.IO.Stream GetFileStream(string path, FileMode fileMode, FileAccess fileAccess)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return new FileStream(path, fileMode, fileAccess);
        }

        public System.IO.Stream GetMemoryStream()
        {
            return new MemoryStream();
        }

        #endregion
    }
}
