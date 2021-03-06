﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZigZag.Editor.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Shaders {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Shaders() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ZigZag.Editor.Resources.Shaders", typeof(Shaders).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform sampler2D active_texture;
        ///
        ///in vec2 uv;
        ///in vec4 color;
        ///
        ///out vec4 fragment_color;
        ///
        ///void main()
        ///{
        ///    //fragment_color = texture(active_texture, uv);
        ///    //color *
        ///    fragment_color = color * texture(active_texture, uv);
        ///}
        ///.
        /// </summary>
        internal static string ImGuiFragmentShaderSource {
            get {
                return ResourceManager.GetString("ImGuiFragmentShaderSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout(location = 0) in vec2 pos_in;
        ///layout(location = 1) in vec2 uv_in;
        ///layout(location = 2) in vec4 color_in;
        ///
        ///out vec2 uv;
        ///out vec4 color;
        ///
        ///uniform vec2 viewport_min;
        ///uniform vec2 viewport_max;
        ///
        ///vec2 map(vec2 value, vec2 fromMin, vec2 fromMax, vec2 toMin, vec2 toMax)
        ///{
        ///  return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        ///}
        ///
        ///void main(void)
        ///{
        ///    vec2 pos = map(pos_in, viewport_min, viewport_max, vec2(-1, 1), vec2(1, -1));
        ///    gl_Position = vec4(pos, 0.0, 1.0);
        ///   [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ImGuiVertexShaderSource {
            get {
                return ResourceManager.GetString("ImGuiVertexShaderSource", resourceCulture);
            }
        }
    }
}
