using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Xpand.ExpressApp")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Xpand.ExpressApp")]
[assembly: AssemblyCopyright("Copyright © 2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("507db8bb-1411-4f9d-bd49-61f65578ac27")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion(XpandAssemblyInfo.Version)]
[assembly: AssemblyFileVersion(XpandAssemblyInfo.FileVersion)]
[assembly: InternalsVisibleTo("eXpand.Tests,  PublicKey=0024000004800000940000000602000000240000525341310004000001000100df18f4f3de9ec490707183c78a72914070a526bfb1818e1687442b137c2bfa9bf5e8533859a8efaa62aa2ea28e03623fef5531f8dd29d74f781a9e50743172dbe8d74b0106ceddfcda17f8dd1034f2896a56e1026faa2cc0e2def8dc1f519ad13924c44f16339a57ed97981a8777c7fa6025a11e54cc694e504d462a400681c0"), InternalsVisibleTo("DynamicMockAssembly,  PublicKey=0024000004800000940000000602000000240000525341310004000001000100ab8e3015b99a732d20ecb2a29fb3f54288a8a614896e7c5091d7b9045368fe6b8bfcc72dce4f01b71281eb4e380dcb709c83a5042a54c684a4711248c078fefb01bcdb09a6ce252e0304ed08c6e4ddf69212e3d0a770d953572e3c474fc08fe3bdbb2fad97b32c6045c08f34466dc8e07bd255d3dbc72408dce6859edb4b04bf")]
[assembly: InternalsVisibleTo("Xpand.ExpressApp.Win, PublicKey=0024000004800000940000000602000000240000525341310004000001000100df18f4f3de9ec490707183c78a72914070a526bfb1818e1687442b137c2bfa9bf5e8533859a8efaa62aa2ea28e03623fef5531f8dd29d74f781a9e50743172dbe8d74b0106ceddfcda17f8dd1034f2896a56e1026faa2cc0e2def8dc1f519ad13924c44f16339a57ed97981a8777c7fa6025a11e54cc694e504d462a400681c0")]
[assembly: InternalsVisibleTo("Xpand.ExpressApp.Web, PublicKey=0024000004800000940000000602000000240000525341310004000001000100df18f4f3de9ec490707183c78a72914070a526bfb1818e1687442b137c2bfa9bf5e8533859a8efaa62aa2ea28e03623fef5531f8dd29d74f781a9e50743172dbe8d74b0106ceddfcda17f8dd1034f2896a56e1026faa2cc0e2def8dc1f519ad13924c44f16339a57ed97981a8777c7fa6025a11e54cc694e504d462a400681c0")]
[assembly: InternalsVisibleTo("Xpand.ExpressApp.Security, PublicKey=0024000004800000940000000602000000240000525341310004000001000100df18f4f3de9ec490707183c78a72914070a526bfb1818e1687442b137c2bfa9bf5e8533859a8efaa62aa2ea28e03623fef5531f8dd29d74f781a9e50743172dbe8d74b0106ceddfcda17f8dd1034f2896a56e1026faa2cc0e2def8dc1f519ad13924c44f16339a57ed97981a8777c7fa6025a11e54cc694e504d462a400681c0")]
[assembly: InternalsVisibleTo("Xpand.ExpressApp.ModelDifference, PublicKey=0024000004800000940000000602000000240000525341310004000001000100df18f4f3de9ec490707183c78a72914070a526bfb1818e1687442b137c2bfa9bf5e8533859a8efaa62aa2ea28e03623fef5531f8dd29d74f781a9e50743172dbe8d74b0106ceddfcda17f8dd1034f2896a56e1026faa2cc0e2def8dc1f519ad13924c44f16339a57ed97981a8777c7fa6025a11e54cc694e504d462a400681c0")]
[assembly: InternalsVisibleTo("Xpand.ExpressApp.IO, PublicKey=0024000004800000940000000602000000240000525341310004000001000100df18f4f3de9ec490707183c78a72914070a526bfb1818e1687442b137c2bfa9bf5e8533859a8efaa62aa2ea28e03623fef5531f8dd29d74f781a9e50743172dbe8d74b0106ceddfcda17f8dd1034f2896a56e1026faa2cc0e2def8dc1f519ad13924c44f16339a57ed97981a8777c7fa6025a11e54cc694e504d462a400681c0")]
[assembly: AllowPartiallyTrustedCallers]

internal class InternalAssemblyInfo {
    public const string Version = "12.2.6.3";
}