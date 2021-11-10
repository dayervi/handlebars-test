using System.Net.Mime;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Controller]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        
        private const string _testLayout = @"
<!DOCTYPE html>
<html>
    <head>
        <style>
            .no-border { border: 1px #dddddd; }
            .thead { background-color: #efefef; }
            .header-content {
                background: grey;
                padding: 5px;
                margin: 5px;
                text-align: center;
            }
        </style>
    </head>
    <body>
        {{> content}}
    </body>
</html>
        ";
        
        private readonly IHandlebars _engine;

        public TestController(IHandlebars engine)
        {
            _engine = engine;
        }

        
        [HttpGet("simple")]
        public ActionResult SimpleTest()
        {
            const string content = @"<p>Hello {{ name }}!</p>";
            _engine.RegisterTemplate("content", content);

            var template = _engine.Compile(_testLayout);
            return new ContentResult
            {
                ContentType = MediaTypeNames.Text.Html,
                Content = template(new
                {
                    name = "John Doe"
                })
            };
        }

        
        [HttpGet("partials")]
        public ActionResult PartialsTest()
        {
            const string list = @"
<ul>
    {{#values}}
        <li>{{this}}</li>
    {{/values}}
</ul>
            ";
            const string content = @"
<div class='header-content'>
    <h3>Some header content here!</h3>
</div>
<p>Bonjour {{firstName}} {{lastName}}</p>
{{#if values}}
{{> list}}
{{/if}}
<div>
            ";
            
            _engine.RegisterTemplate("list", list);
            _engine.RegisterTemplate("content", content);

            var template = _engine.Compile(_testLayout);
            return new ContentResult
            {
                ContentType = MediaTypeNames.Text.Html,
                Content = template(new
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Values = new[] { "item 1", "item 2", "item 3" }
                })
            };

        }
        
    }
    
}