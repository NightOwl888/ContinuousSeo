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

namespace ContinuousSeo.Core.Net
{
    using System;
    using System.Collections.Specialized;
    using System.IO;

    /// <summary>
    /// Interface for HttpClient.
    /// </summary>
    public interface IHttpClient
    {
        NameValueCollection Get(Stream output, string url);
        NameValueCollection Post(Stream output, string url, string data);
        Stream GetResponseStream(string url);
        void Close();
        void Dispose();
        string GetResponseText(string url);
    }
}
