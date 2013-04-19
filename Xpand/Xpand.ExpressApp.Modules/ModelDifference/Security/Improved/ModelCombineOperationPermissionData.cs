using System.Collections.Generic;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using Xpand.ExpressApp.Security.Permissions;

namespace Xpand.ExpressApp.ModelDifference.Security.Improved {
    [System.ComponentModel.DisplayName("ModelCombine")]
    public class ModelCombineOperationPermissionData : XpandPermissionData, IModelCombinePermission {

        private ApplicationModelCombineModifier modifier = ApplicationModelCombineModifier.Allow;

        public ModelCombineOperationPermissionData(Session session)
            : base(session) {
        }

        public override IList<IOperationPermission> GetPermissions() {
            return new IOperationPermission[] { new ModelCombineOperationPermission(modifier, Difference) };
        }

        public ApplicationModelCombineModifier Modifier {
            get { return modifier; }
            set { modifier = value; }
        }
        private string _difference;
        public string Difference {
            get {
                return _difference;
            }
            set {
                SetPropertyValue("Difference", ref _difference, value);
            }
        }
    }
}