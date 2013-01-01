// Forwarding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Xml.Serialization;

[assembly: TypeForwardedTo(typeof (XmlAnyElementAttributes))]
[assembly: TypeForwardedTo(typeof (XmlArrayItemAttributes))]
[assembly: TypeForwardedTo(typeof (XmlAttributeOverrides))]
[assembly: TypeForwardedTo(typeof (XmlAttributes))]
[assembly: TypeForwardedTo(typeof (XmlElementAttributes))]
[assembly: TypeForwardedTo(typeof (XmlMapping))]
[assembly: TypeForwardedTo(typeof (XmlMappingAccess))]
[assembly: TypeForwardedTo(typeof (XmlSerializer))]
[assembly: TypeForwardedTo(typeof (XmlSerializerNamespaces))]
[assembly: TypeForwardedTo(typeof (XmlTypeMapping))]
[assembly: TypeForwardedTo(typeof (XmlSerializerFormatAttribute))]