#pragma checksum "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\Home\Privacy.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3ecd94ed7a45773cac75829f403773b757472774"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Privacy), @"mvc.1.0.view", @"/Views/Home/Privacy.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\_ViewImports.cshtml"
using DependencyInjectionMVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\_ViewImports.cshtml"
using DependencyInjectionMVC.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3ecd94ed7a45773cac75829f403773b757472774", @"/Views/Home/Privacy.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4355a7a683bfc96a4f5b3c293f9b2d8c73299c80", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Privacy : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<Post>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\Home\Privacy.cshtml"
  
    ViewData["Title"] = "Post List";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"m-1\">\r\n    <a class=\"btn btn-success\"");
            BeginWriteAttribute("href", " href=\"", 115, "\"", 142, 1);
#nullable restore
#line 7 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\Home\Privacy.cshtml"
WriteAttributeValue("", 122, Url.Action("Index"), 122, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("> Create Post</a>\r\n</div>\r\n\r\n\r\n<div class=\"card-columns card-group\">\r\n");
#nullable restore
#line 12 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\Home\Privacy.cshtml"
      
        foreach (var item in Model)
        {
            var content = item.Content.Length > 300 ? item.Content.Substring(0, 300) + "..." : item.Content;


#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"card\">\r\n                <div class=\"card-body\">\r\n                    <h5>");
#nullable restore
#line 19 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\Home\Privacy.cshtml"
                   Write(item.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                    <h6>Create Post Date on ");
#nullable restore
#line 20 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\Home\Privacy.cshtml"
                                       Write(item.PostDateTime.ToShortDateString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n                    <p class=\"card-text\">");
#nullable restore
#line 21 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\Home\Privacy.cshtml"
                                    Write(item.Content);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                </div>\r\n                <div class=\"card-footer\">\r\n                    <small class=\"text-muted\">More ...</small>\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 27 "C:\Users\ABRA\Desktop\Github\.NetCoreDependencyInjection\DependencyInjectionMVC\DependencyInjectionMVC\Views\Home\Privacy.cshtml"
        }
    

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<Post>> Html { get; private set; }
    }
}
#pragma warning restore 1591