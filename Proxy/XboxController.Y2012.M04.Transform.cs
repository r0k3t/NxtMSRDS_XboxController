//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: global::System.Reflection.AssemblyProductAttribute("XboxController")]
[assembly: global::System.Reflection.AssemblyTitleAttribute("XboxController")]
[assembly: global::Microsoft.Dss.Core.Attributes.ServiceDeclarationAttribute(global::Microsoft.Dss.Core.Attributes.DssServiceDeclaration.Transform, SourceAssemblyKey="XboxController.Y2012.M04, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ad9139" +
    "f29e19437f")]
[assembly: global::System.Security.SecurityTransparentAttribute()]
[assembly: global::System.Security.AllowPartiallyTrustedCallersAttribute()]
[assembly: global::System.Security.SecurityRulesAttribute(global::System.Security.SecurityRuleSet.Level1)]

namespace Dss.Transforms.TransformXboxController {
    
    
    public class Transforms : global::Microsoft.Dss.Core.Transforms.TransformBase {
        
        static Transforms() {
            Register();
        }
        
        public static void Register() {
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::XboxController.Proxy.XboxControllerState), new global::Microsoft.Dss.Core.Attributes.Transform(XboxController_Proxy_XboxControllerState_TO_XboxController_XboxControllerState));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::XboxController.XboxControllerState), new global::Microsoft.Dss.Core.Attributes.Transform(XboxController_XboxControllerState_TO_XboxController_Proxy_XboxControllerState));
        }
        
        private static global::XboxController.Proxy.XboxControllerState _cachedInstance0 = new global::XboxController.Proxy.XboxControllerState();
        
        private static global::XboxController.XboxControllerState _cachedInstance = new global::XboxController.XboxControllerState();
        
        public static object XboxController_Proxy_XboxControllerState_TO_XboxController_XboxControllerState(object transformFrom) {
            return _cachedInstance;
        }
        
        public static object XboxController_XboxControllerState_TO_XboxController_Proxy_XboxControllerState(object transformFrom) {
            return _cachedInstance0;
        }
    }
}
