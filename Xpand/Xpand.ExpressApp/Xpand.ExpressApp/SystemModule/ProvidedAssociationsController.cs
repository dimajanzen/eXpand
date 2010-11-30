using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Xpand.ExpressApp.Attributes;
using Xpand.ExpressApp.Core;
using Xpand.Persistent.Base.General;

namespace Xpand.ExpressApp.SystemModule
{
    public class ProvidedAssociationsController : Controller
    {

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            IEnumerable<IMemberInfo> memberInfos =
                typesInfo.PersistentTypes.SelectMany(typeInfo => typeInfo.OwnMembers);
            foreach (var memberInfo in memberInfos.Where(memberInfo => memberInfo.FindAttribute<ProvidedAssociationAttribute>() != null))
            {
                var providedAssociationAttribute = memberInfo.FindAttribute<ProvidedAssociationAttribute>();
                AssociationAttribute associationAttribute = GetAssociationAttribute(memberInfo, providedAssociationAttribute);
                XPCustomMemberInfo customMemberInfo = CreateMemberInfo(typesInfo, memberInfo, providedAssociationAttribute, associationAttribute);
                if (!(string.IsNullOrEmpty(providedAssociationAttribute.AttributesFactoryProperty)))
                    foreach (var attribute in GetAttributes(providedAssociationAttribute.AttributesFactoryProperty, memberInfo.Owner))
                    {
                        customMemberInfo.AddAttribute(attribute);
                    }
            }
        }

        AssociationAttribute GetAssociationAttribute(IMemberInfo memberInfo, ProvidedAssociationAttribute providedAssociationAttribute)
        {
            var associationAttribute = memberInfo.FindAttribute<AssociationAttribute>();
            if (associationAttribute == null && !string.IsNullOrEmpty(providedAssociationAttribute.AssociationName))
                associationAttribute = new AssociationAttribute(providedAssociationAttribute.AssociationName);
            else if (associationAttribute == null)
                throw new NullReferenceException(memberInfo + " has no association attribute");
            return associationAttribute;
        }

        IEnumerable<Attribute> GetAttributes(string attributesFactoryProperty, ITypeInfo owner)
        {
            PropertyInfo memberInfo = owner.Type.GetProperty(attributesFactoryProperty);
            return memberInfo != null ? (IEnumerable<Attribute>)memberInfo.GetValue(null, null) : new List<Attribute>();
        }

        private XPCustomMemberInfo CreateMemberInfo(ITypesInfo typesInfo, IMemberInfo memberInfo, ProvidedAssociationAttribute providedAssociationAttribute, AssociationAttribute associationAttribute)
        {
            var typeToCreateOn = getTypeToCreateOn(memberInfo, associationAttribute);
            if (typeToCreateOn == null)
                throw new NotImplementedException();
            XPCustomMemberInfo xpCustomMemberInfo;
            if (!(memberInfo.IsList) || (memberInfo.IsList && providedAssociationAttribute.RelationType == RelationType.ManyToMany))
            {
                xpCustomMemberInfo = typesInfo.CreateCollection(
                    typeToCreateOn, 
                    memberInfo.Owner.Type, 
                    associationAttribute.Name, 
                    XafTypesInfo.XpoTypeInfoSource.XPDictionary,
                    providedAssociationAttribute.ProvidedPropertyName ?? memberInfo.Owner.Type.Name + "s", false);
            }
            else
            {
                xpCustomMemberInfo = typesInfo.CreateMember(
                    typeToCreateOn,
                    memberInfo.Owner.Type,
                    associationAttribute.Name,
                    XafTypesInfo.XpoTypeInfoSource.XPDictionary,
                    providedAssociationAttribute.ProvidedPropertyName ?? memberInfo.Owner.Type.Name, false);
            }

            if (!string.IsNullOrEmpty(providedAssociationAttribute.AssociationName) && memberInfo.FindAttribute<AssociationAttribute>() == null)
                memberInfo.AddAttribute(new AssociationAttribute(providedAssociationAttribute.AssociationName));

            typesInfo.RefreshInfo(typeToCreateOn);

            return xpCustomMemberInfo;
        }

        private Type getTypeToCreateOn(IMemberInfo memberInfo, AssociationAttribute associationAttribute)
        {
            if (!memberInfo.MemberType.IsGenericType)
                return string.IsNullOrEmpty(associationAttribute.ElementTypeName) ? memberInfo.MemberType : Type.GetType(associationAttribute.ElementTypeName);
            return memberInfo.MemberType.GetGenericArguments()[0];
        }
    }
}