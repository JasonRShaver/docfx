// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.MarkdigEngine.Tests
{
    using MarkdigEngine;

    using Microsoft.DocAsCode.Plugins;
    using System.Collections.Generic;
    using Xunit;

    public class AladdinTest
    {

        [Fact]
        public void TestAladdin_General()
        {
            var content = @"[!aladdin[page1](section1)]";
            var expected = @"
<div id=page1.aladdin-hook class='aladdin-hook' title='Related Questions'>
   <div class='question-border'></div>
   <div style='display:inline-block'>
      <a class='question-button question-icon' id='page1.aladdin-hook-questions-icon'>
         Related Questions <div><i class='ms-Icon ms-Icon--ChevronDown'></i></div>
      </a>
   </div>
   <div class='question-border'></div>
</div>";

            var parameter = new MarkdownServiceParameters
            {
                BasePath = ".",
                Extensions = new Dictionary<string, object>
                {
                    { "EnableAladdin", true},
                    { "EnableSourceInfo", false }
                }
            };
            var service = new MarkdigMarkdownService(parameter);


            var marked = service.Markup(content, string.Empty);
            
            Assert.Equal(expected, marked.Html);
        }

    }
}
