#region Copyright

// <copyright file="Forwarding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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